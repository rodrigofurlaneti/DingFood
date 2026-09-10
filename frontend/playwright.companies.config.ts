import { defineConfig } from "@playwright/test";

export default defineConfig({
    testDir: "./test/features/companies",
    use: { baseURL: "http://localhost:5188", browserName: "chromium" },
    webServer: {
        command: "node node_modules/vite/bin/vite.js --host 127.0.0.1 --port 5188 --strictPort",
        url: "http://localhost:5188",
        reuseExistingServer: false,
    },
});
