import { onlyDigits } from "./cnpjUtils";

/** Formata para exibição. "01001000" -> "01001-000" */
export function formatCep(value?: string | null): string {
    const digits = onlyDigits(value);
    if (digits.length !== 8) return digits;
    return `${digits.slice(0, 5)}-${digits.slice(5)}`;
}

/**
 * Valida o formato do CEP — mesmo critério do CepValidator no backend.
 * Oito dígitos, e não uma repetição do mesmo dígito ("00000000" nunca existe na base).
 * O ViaCEP devolve 400 para formato inválido e bloqueia acesso por uso massivo, então
 * vale filtrar antes de sair da tela.
 */
export function isValidCep(value?: string | null): boolean {
    const digits = onlyDigits(value);
    if (digits.length !== 8) return false;
    return !/^(\d)\1{7}$/.test(digits);
}
