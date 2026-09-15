import { api, ApiError } from "../../lib/apiClient";
import { onlyDigits } from "./cnpjUtils";

/**
 * Contrato de GET /api/cnpj/{taxId} — espelha o CnpjResponse do backend
 * (DingFood.Application.Features.Cnpj.CnpjResponse), serializado em camelCase.
 */
export interface CnpjActivity {
    code: number | null;
    text: string | null;
}

export interface CnpjMember {
    name: string | null;
    /** CPF mascarado na origem ("***765288**") — não serve para preencher campo de CPF. */
    taxId: string | null;
    /** "NATURAL" (pessoa física) ou "LEGAL" (pessoa jurídica). */
    personType: string | null;
    role: string | null;
    since: string | null;
    ageRange: string | null;
    country: string | null;
}

export interface CnpjAddress {
    street: string | null;
    number: string | null;
    details: string | null;
    district: string | null;
    city: string | null;
    state: string | null;
    zip: string | null;
    municipalityCode: number | null;
}

export interface CnpjPhone {
    type: string | null;
    area: string | null;
    number: string | null;
}

export interface CnpjEmail {
    ownership: string | null;
    address: string | null;
    domain: string | null;
}

export interface CnpjLookupResponse {
    taxId: string;
    taxIdFormatted: string;
    legalName: string;
    tradeName: string | null;
    foundedOn: string | null;
    isHeadOffice: boolean;
    statusId: number | null;
    statusText: string | null;
    statusDate: string | null;
    reasonText: string | null;
    natureText: string | null;
    sizeAcronym: string | null;
    sizeText: string | null;
    equity: number | null;
    simplesOptant: boolean | null;
    simplesSince: string | null;
    simeiOptant: boolean | null;
    simeiSince: string | null;
    mainActivity: CnpjActivity | null;
    sideActivities: CnpjActivity[];
    members: CnpjMember[];
    address: CnpjAddress | null;
    phones: CnpjPhone[];
    emails: CnpjEmail[];
    sourceUpdatedAt: string | null;
    queriedAt: string;
    ageInDays: number;
    /** true quando veio do cache do banco, sem bater na CNPJá. */
    fromCache: boolean;
    /** true quando a CNPJá estava indisponível e servimos um dado vencido. */
    staleData: boolean;
}

/** Consulta o CNPJ na API do DingFood. Endpoint anônimo — a tela de cadastro é pré-login. */
export function lookupCnpj(cnpj: string, signal?: AbortSignal): Promise<CnpjLookupResponse> {
    return api<CnpjLookupResponse>(`/api/cnpj/${onlyDigits(cnpj)}`, { signal });
}

/** Traduz os Error.Code do backend em mensagens que fazem sentido para quem está se cadastrando. */
export function describeCnpjError(error: unknown): string {
    if (!(error instanceof ApiError)) {
        return "Não foi possível consultar o CNPJ. Preencha os dados manualmente.";
    }

    switch (error.code) {
        case "Cnpj.Invalid":
            return "CNPJ inválido — confira os dígitos.";
        case "Cnpj.NotFound":
            return "CNPJ não encontrado na base da Receita Federal.";
        case "Cnpja.RateLimited":
            return "Muitas consultas seguidas. Aguarde alguns segundos e tente de novo.";
        case "Cnpja.Timeout":
        case "Cnpja.Unavailable":
            return "A consulta de CNPJ está indisponível agora. Preencha os dados manualmente.";
        case "Network.Unreachable":
            return error.message;
        default:
            return "Não foi possível consultar o CNPJ. Preencha os dados manualmente.";
    }
}
