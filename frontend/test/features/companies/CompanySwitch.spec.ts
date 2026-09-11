import { test, expect } from "@playwright/test";

test("switches JWT, clears old form state and selects only the new company's branch", async ({ page }) => {
    await page.addInitScript(() => {
        if (!localStorage.getItem("syncbar-auth")) localStorage.setItem("syncbar-auth", JSON.stringify({ state: {
            accessToken: "token-a", refreshToken: "refresh", userName: "Admin", companyId: 10, homeCompanyId: 10,
            businessGroupId: 1, branchId: 100, employeeId: 1,
        }, version: 0 }));
    });
    const companyHeaders: string[] = [];
    const created: unknown[] = [];
    await page.route("**/api/**", async route => {
        const request = route.request();
        const path = new URL(request.url()).pathname;
        let body: unknown = [];
        if (path === "/api/companies/allowed") body = [
            { companyId: 10, businessGroupId: 1, groupName: "Cozinha", tradeName: "Burger", employeeId: 1, roles: ["Administrador"] },
            { companyId: 20, businessGroupId: 1, groupName: "Cozinha", tradeName: "Pizza", employeeId: 2, roles: ["Administrador"] },
        ];
        if (path === "/api/workplaces" && request.method() === "GET") {
            companyHeaders.push(request.headers()["x-company-id"]);
            body = request.headers()["x-company-id"] === "10"
                ? [{ id: 100, name: "Filial Burger", isActive: true, brandId: 1, brandName: "Burger", employeeId: 1 }]
                : [{ id: 200, name: "Filial Pizza", isActive: true, brandId: 2, brandName: "Pizza", employeeId: 2 }];
        }
        if (path === "/api/workplaces/brands") body = [{ id: 1, name: "Burger" }, { id: 2, name: "Pizza" }];
        if (path === "/api/workplaces" && request.method() === "POST") {
            expect(request.headers()["x-company-id"]).toBe("20");
            created.push(request.postDataJSON());
            expect(request.postDataJSON()).toEqual(created.length === 1
                ? { brandName: "Sushi", branchName: "Centro", existingBrandId: null }
                : { brandName: "", branchName: "Norte", existingBrandId: 2 });
            body = 300;
        }
        if (path === "/api/access/my-features") body = { canManageAccess: true, features: ["Salao", "Cardapio"] };
        if (path.startsWith("/api/comandas/settings/")) body = { defaultLimitAmount: 0 };
        if (path === "/api/companies/switch") {
            expect(request.postDataJSON()).toEqual({ targetCompanyId: 20 });
            body = { accessToken: "token-b", companyId: 20, businessGroupId: 1, employeeId: 2 };
        }
        await route.fulfill({ json: body });
    });
    await page.goto("/empresas");
    await page.getByRole("button", { name: "Selecionar empresa e filial" }).click();
    await expect(page.getByLabel("Empresa", { exact: true })).toHaveValue("10");
    await page.getByLabel("Razão social").fill("Formulário da Burger");
    await page.getByRole("button", { name: "Selecionar empresa e filial" }).click();
    await page.getByLabel("Empresa", { exact: true }).selectOption("20");
    await expect(page.getByRole("button", { name: "Selecionar empresa e filial" })).toContainText("Pizza");
    await page.getByRole("button", { name: "Selecionar empresa e filial" }).click();
    await expect(page.getByLabel("Empresa", { exact: true })).toHaveValue("20");
    await expect(page.getByLabel("Filial", { exact: true })).toHaveValue("200");
    await expect(page.getByRole("option", { name: "Filial Burger" })).toHaveCount(0);
    const state = await page.evaluate(() => JSON.parse(localStorage.getItem("syncbar-auth")!).state);
    expect(state.accessToken).toBe("token-b");
    expect(state.businessGroupId).toBe(1);
    expect(state.companyId).toBe(20);
    expect(state.employeeId).toBe(2);
    expect(companyHeaders).toContain("20");
    await page.goto("/empresas");
    await expect(page.getByLabel("Razão social")).toHaveValue("");
    await page.getByRole("button", { name: "+ Nova marca", exact: true }).click();
    await page.getByLabel("Nome da marca").fill("Sushi");
    await page.getByLabel("Primeira filial da marca").fill("Centro");
    await page.getByRole("button", { name: "Criar marca", exact: true }).click();
    await expect(page.getByRole("status")).toContainText("Cadastro concluído");
    await page.getByRole("button", { name: "+ Nova filial", exact: true }).click();
    await page.getByRole("combobox", { name: "Marca", exact: true }).selectOption("2");
    await expect(page.getByRole("button", { name: "Criar filial", exact: true })).toBeEnabled();
    await page.getByLabel("Nome da nova filial").fill("Norte");
    await page.getByRole("button", { name: "Criar filial", exact: true }).click();
    await expect.poll(() => created.length).toBe(2);
    await expect(page.getByLabel("Nome da nova filial")).toHaveValue("");
    await page.setViewportSize({ width: 1920, height: 1080 });
    await page.screenshot({ path: "test-results/companies-desktop.png", fullPage: true, animations: "disabled" });
    await page.setViewportSize({ width: 390, height: 844 });
    await page.getByRole("button", { name: "Selecionar empresa e filial" }).click();
    await expect(page.locator("#company-context-panel")).toBeVisible();
    expect(await page.evaluate(() => document.documentElement.scrollWidth <= innerWidth)).toBe(true);
    await page.screenshot({ path: "test-results/companies-mobile.png", fullPage: true, animations: "disabled" });
    await page.keyboard.press("Escape");
    await expect(page.getByRole("button", { name: "Selecionar empresa e filial" })).toBeFocused();
});
