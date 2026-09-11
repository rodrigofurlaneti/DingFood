import { defineConfig } from '@playwright/test';
export default defineConfig({
  testDir: './test/features/orders', testMatch: 'OwnDelivery.spec.ts',
  use: { baseURL: 'http://127.0.0.1:5189', viewport: { width: 1280, height: 900 } },
  webServer: { command: 'node node_modules/vite/bin/vite.js --host 127.0.0.1 --port 5189', url: 'http://127.0.0.1:5189', reuseExistingServer: true },
});
