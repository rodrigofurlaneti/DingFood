import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './test-live',
  testMatch: 'storefront.spec.ts',
  workers: 1,
  retries: 0, // A failed registration must not silently create additional customers.
  timeout: 90000,
  reporter: 'list',
  use: {
    baseURL: process.env.STOREFRONT_BASE_URL ?? 'http://191.234.174.58:80',
    ignoreHTTPSErrors: false,
    trace: 'off', // Registration requests contain the generated password.
    screenshot: 'only-on-failure',
  },
});
