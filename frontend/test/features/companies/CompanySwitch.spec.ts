import { test, expect } from "@playwright/test";

test("switches JWT, clears old form state and selects only the new company's branch", async ({ page }) => {
    await page.addInitScript(() => {
        if (!localStorage.getItem("syncbar-auth")) localStorage.setItem("syncbar-auth", JSON.stringify({ state: {
            accessToken: "token-a", refreshToken: "refresh", userName: "Admin", companyId: 10, homeCompanyId: 10,
            businessGroupId: 1, branchId: 100, employeeId: 1,
        }, version: 0 }));
    });
    const companyHeaders: string[] = [];
    await page.route("**/api/**", async route => {
        const request = route.request();
        const path = new URL(request.url()).pathname;
        let body: unknown = [];
        if (path === "/api/companies/allowed") body = [
            { companyId: 10, businessGroupId: 1, groupName: "Cozinha", tradeName: "Burger", employeeId: 1, roles: ["Administrador"] },
            { companyId: 20, businessGroupId: 1, groupName: "Cozinha", tradeName: "Pizza", employeeId: 2, roles: ["Administrador"] },
        ];
        if (path === "/api/branches/company/10") body = [{ id: 100, name: "Filial Burger", isActive: true }];
        if (path === "/api/branches/company/20") {
            companyHeaders.push(request.headers()["x-company-id"]);
            body = [{ id: 200, name: "Filial Pizza", isActive: true }];
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
    await expect(page.getByLabel("Empresa", { exact: true })).toHaveValue("10");
    await page.getByLabel("Razão social").fill("Formulário da Burger");
    await page.getByLabel("Empresa", { exact: true }).selectOption("20");
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
});
