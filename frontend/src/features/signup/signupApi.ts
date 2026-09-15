import { api } from "../../lib/apiClient";
import { SignupFormData } from "./signupSchema";

export function signupApi(data: SignupFormData) {
    return api("/api/slideup/register", {
        method: "POST",
        body: JSON.stringify({
            legalName: data.legalName,
            tradeName: data.tradeName,
            cnpj: data.cnpj,
            branchName: data.branchName,
            adminName: data.adminName,
            adminCpf: data.adminCpf,
            adminUserName: data.adminUserName,
            adminEmail: data.adminEmail,
            adminPassword: data.adminPassword,
        }),
    });
}

/**
 * Verifica se um usuário já existe (opcional — não é chamado hoje pelo form)
 */
export async function checkUsernameAvailability(username: string): Promise<boolean> {
    try {
        const data = await api<{ available: boolean }>(
            `/api/auth/check-username?username=${encodeURIComponent(username)}`
        );
        return data.available === true;
    } catch {
        return false;
    }
}

/**
 * Verifica se um email já está registrado (opcional — não é chamado hoje pelo form)
 */
export async function checkEmailAvailability(email: string): Promise<boolean> {
    try {
        const data = await api<{ available: boolean }>(
            `/api/auth/check-email?email=${encodeURIComponent(email)}`
        );
        return data.available === true;
    } catch {
        return false;
    }
}