import { api } from "../../lib/apiClient";
import { SignupFormData } from "./signupSchema";

/** Converte string vazia/whitespace em undefined, para não mandar "" nos campos opcionais. */
function orUndefined(value?: string): string | undefined {
    return value && value.trim() !== "" ? value : undefined;
}

/**
 * Registra uma nova empresa, a primeira filial (Matriz) e o usuário
 * administrador em uma única transação.
 *
 * Endpoint real: POST /api/slideup/register
 * Payload compatível com RegisterCompanyCommand (backend).
 *
 * `companyEmail` não tem campo próprio no formulário — usamos o mesmo valor
 * de `adminEmail`, a pedido do usuário. Os demais campos opcionais do command
 * (telefone, CNPJ da filial, endereço) são coletados pela seção "opcional"
 * da tela e enviados como undefined quando ficam em branco (o backend aceita
 * null neles).
 */
export function signupApi(data: SignupFormData) {
    return api("/api/slideup/register", {
        method: "POST",
        body: JSON.stringify({
            legalName: data.legalName,
            tradeName: data.tradeName,
            cnpj: data.cnpj,
            companyEmail: data.adminEmail,
            companyPhone: orUndefined(data.companyPhone),
            branchName: data.branchName,
            branchCnpj: orUndefined(data.branchCnpj),
            addressStreet: orUndefined(data.addressStreet),
            addressNumber: orUndefined(data.addressNumber),
            addressDistrict: orUndefined(data.addressDistrict),
            addressCity: orUndefined(data.addressCity),
            addressState: orUndefined(data.addressState?.toUpperCase()),
            addressZipCode: orUndefined(data.addressZipCode),
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