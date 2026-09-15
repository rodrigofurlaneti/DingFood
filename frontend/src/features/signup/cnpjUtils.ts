/** Remove tudo que não for dígito. "14.486.046/0001-77" -> "14486046000177" */
export function onlyDigits(value?: string | null): string {
    return (value ?? "").replace(/\D/g, "");
}

/** Formata para exibição. "14486046000177" -> "14.486.046/0001-77" */
export function formatCnpj(value?: string | null): string {
    const digits = onlyDigits(value);
    if (digits.length !== 14) return digits;
    return `${digits.slice(0, 2)}.${digits.slice(2, 5)}.${digits.slice(5, 8)}/${digits.slice(8, 12)}-${digits.slice(12)}`;
}

const FIRST_WEIGHTS = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
const SECOND_WEIGHTS = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

function checkDigit(digits: string, weights: number[]): number {
    const sum = weights.reduce((acc, weight, index) => acc + Number(digits[index]) * weight, 0);
    const remainder = sum % 11;
    return remainder < 2 ? 0 : 11 - remainder;
}

/**
 * Valida os dígitos verificadores do CNPJ — mesmo algoritmo do CnpjValidator no backend.
 * Evita disparar uma consulta (e gastar a cota de 5/min da API pública) para um número
 * que já sabemos ser inválido.
 */
export function isValidCnpj(value?: string | null): boolean {
    const digits = onlyDigits(value);
    if (digits.length !== 14) return false;
    if (/^(\d)\1{13}$/.test(digits)) return false;
    return (
        Number(digits[12]) === checkDigit(digits, FIRST_WEIGHTS) &&
        Number(digits[13]) === checkDigit(digits, SECOND_WEIGHTS)
    );
}

/**
 * Telefone fixo tem 10 dígitos, celular tem 11. A máscara precisa acompanhar, senão
 * um fixo vindo da Receita entra incompleto num campo de 11 posições.
 */
export function phoneMaskFor(value?: string | null): string {
    return onlyDigits(value).length > 10 ? "(99) 99999-9999" : "(99) 9999-9999";
}
