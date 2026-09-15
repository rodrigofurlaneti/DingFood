import { api, ApiError } from "../../lib/apiClient";
import { onlyDigits } from "./cnpjUtils";

/**
 * Contrato de GET /api/cep/{cep} — espelha o CepResponse do backend
 * (DingFood.Application.Features.Cep.CepResponse), serializado em camelCase.
 */
export interface CepLookupResponse {
    cep: string;
    cepFormatted: string;
    /** Vem vazio em CEP de cidade inteira (ex.: 01000-000), o que é legítimo. */
    street: string | null;
    complement: string | null;
    unit: string | null;
    district: string | null;
    city: string | null;
    stateAbbreviation: string | null;
    stateName: string | null;
    region: string | null;
    ibgeCode: number | null;
    giaCode: string | null;
    areaCode: string | null;
    siafiCode: string | null;
    queriedAt: string;
    ageInDays: number;
    /** true quando veio do cache do banco, sem bater no ViaCEP. */
    fromCache: boolean;
    /** true quando o ViaCEP estava indisponível e servimos um dado vencido. */
    staleData: boolean;
}

/** Consulta o CEP na API do DingFood. Endpoint anônimo — a tela de cadastro é pré-login. */
export function lookupCep(cep: string, signal?: AbortSignal): Promise<CepLookupResponse> {
    return api<CepLookupResponse>(`/api/cep/${onlyDigits(cep)}`, { signal });
}

/** Traduz os Error.Code do backend em mensagens para quem está preenchendo o cadastro. */
export function describeCepError(error: unknown): string {
    if (!(error instanceof ApiError)) {
        return "Não foi possível consultar o CEP. Preencha o endereço manualmente.";
    }

    switch (error.code) {
        case "Cep.Invalid":
            return "CEP inválido — confira os dígitos.";
        case "Cep.NotFound":
            return "CEP não encontrado na base dos Correios.";
        case "ViaCep.RateLimited":
            return "Muitas consultas seguidas. Aguarde alguns segundos e tente de novo.";
        case "ViaCep.Timeout":
        case "ViaCep.Unavailable":
            return "A consulta de CEP está indisponível agora. Preencha o endereço manualmente.";
        case "Network.Unreachable":
            return error.message;
        default:
            return "Não foi possível consultar o CEP. Preencha o endereço manualmente.";
    }
}
