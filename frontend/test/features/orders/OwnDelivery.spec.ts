import { test, expect } from '@playwright/test';

test('administrator configures rates, registers a driver and assigns an order', async ({ page }) => {
  await page.addInitScript(() => localStorage.setItem('syncbar-auth', JSON.stringify({ state: { accessToken: 'test', companyId: 1, homeCompanyId: 1, branchId: 1, userName: 'Admin' }, version: 0 })));
  const drivers: object[] = [];
  let assigned = false;
  let savedConfig: any = null;
  await page.route('**/api/**', async route => {
    const path = new URL(route.request().url()).pathname;
    if (path === '/api/companies/allowed') return route.fulfill({ json: [{ companyId: 1, businessGroupId: 1, tradeName: 'Test' }] });
    if (path === '/api/workplaces') return route.fulfill({ json: [{ id: 1, name: 'Centro', isActive: true, brandId: 1, brandName: 'Test' }] });
    if (path === '/api/access/my-features') return route.fulfill({ json: { canManageAccess: true, features: [] } });
    if (path === '/api/delivery/drivers') {
      expect(route.request().headers()['x-branch-id']).toBe('1');
      if (route.request().method() === 'POST') { const driver = { ...route.request().postDataJSON(), id: 1 }; drivers.push(driver); return route.fulfill({ json: driver }); }
      return route.fulfill({ json: drivers });
    }
    if (path === '/api/delivery/config') { if (route.request().method() === 'PUT') savedConfig = route.request().postDataJSON(); return route.fulfill({ json: savedConfig }); }
    if (path === '/api/delivery/orders') return route.fulfill({ json: [{ id: 7, customerName: 'Cliente Teste', deliveryDriverId: assigned ? 1 : null, deliveryFeeAmount: 17, deliveryDistanceKm: 2, deliveryPaymentModel: 'PerKm' }] });
    if (path === '/api/delivery/orders/7/driver') { expect(route.request().postDataJSON()).toEqual({ driverId: 1 }); assigned = true; return route.fulfill({ status: 204 }); }
    return route.fulfill({ json: [] });
  });
  await page.goto('/logistica');
  await expect(page.getByRole('heading', { name: 'Motoboys e taxas de entrega' })).toBeVisible();
  await page.getByLabel('Nome', { exact: true }).fill('João Teste');
  await page.getByLabel('Telefone', { exact: true }).fill('11999999999');
  await page.getByLabel('Placa', { exact: true }).fill('ABC1D23');
  await page.getByRole('button', { name: 'Salvar motoboy' }).click();
  await expect(page.getByRole('cell', { name: 'João Teste', exact: true })).toBeVisible();
  await page.getByRole('button', { name: 'Adicionar tarifa dinâmica' }).click();
  await page.getByRole('button', { name: 'Salvar configuração' }).click();
  await expect.poll(() => savedConfig?.conditions.length).toBe(1);
  expect(savedConfig.conditions[0]).toMatchObject({ daysOfWeek: 97, startMinute: 1080, endMinute: 1380, pricePerKm: 8.5 });
  await page.getByLabel('Motoboy do pedido 7').selectOption('1');
  await expect(page.getByLabel('Motoboy do pedido 7')).toHaveCount(0);
  await page.screenshot({ path: 'test-results/own-delivery-desktop.png', fullPage: true });
  await page.setViewportSize({ width: 390, height: 844 });
  await expect(page.getByRole('button', { name: 'Salvar configuração' })).toBeVisible();
  expect(await page.evaluate(() => document.documentElement.scrollWidth <= window.innerWidth)).toBe(true);
  await page.screenshot({ path: 'test-results/own-delivery-mobile.png', fullPage: true });
});
