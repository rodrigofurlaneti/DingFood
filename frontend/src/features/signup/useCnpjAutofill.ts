import { useCallback, useRef, useState } from "react";
import type { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import type { SignupFormData } from "./signupSchema";
import { CnpjLookupResponse, describeCnpjError, lookupCnpj } from "./cnpjApi";
import { isValidCnpj, onlyDigits } from "./cnpjUtils";
import type { AutofillTracker } from "./autofillTracker";

export type CnpjAutofillStatus =
    | { state: "idle" }
    | { state: "loading" }
    | { state: "filled"; data: CnpjLookupResponse; filledCount: number; keptCount: number }
    | { state: "error"; message: string };

/** Campos do formulário que a consulta consegue preencher. */
type FillableField = Extract<
    keyof SignupFormData,
    | "legalName"
    | "tradeName"
    | "companyPhone"
    | "addressZipCode"
    | "addressStreet"
    | "addressNumber"
    | "addressDistrict"
    | "addressCity"
    | "addressState"
    | "adminName"
>;

const FILLABLE_FIELDS: FillableField[] = [
    "legalName",
    "tradeName",
    "companyPhone",
    "addressZipCode",
    "addressStreet",
    "addressNumber",
    "addressDistrict",
    "addressCity",
    "addressState",
    "adminName",
];

/**
 * Escolhe quem vai para o campo "Nome do administrador".
 * Prioriza o sócio cuja qualificação indica administração; se não houver, cai na
 * primeira pessoa física do quadro societário.
 */
function pickAdministrator(members: CnpjMemberLike[]): CnpjMemberLike | undefined {
    const byRole = members.find((m) => /administrador|titular|presidente|diretor/i.test(m.role ?? ""));
    if (byRole) return byRole;
    return members.find((m) => m.personType === "NATURAL") ?? members[0];
}

type CnpjMemberLike = CnpjLookupResponse["members"][number];

function buildPatch(data: CnpjLookupResponse): Partial<Record<FillableField, string>> {
    const phone = data.phones?.[0];
    const admin = pickAdministrator(data.members ?? []);
    const address = data.address;

    const phoneDigits = phone ? onlyDigits(`${phone.area ?? ""}${phone.number ?? ""}`) : "";

    return {
        legalName: data.legalName || undefined,
        // O "alias" (nome fantasia) vem null para boa parte das empresas. Como o campo é
        // obrigatório no formulário, cai na razão social em vez de ficar vazio.
        tradeName: data.tradeName || data.legalName || undefined,
        companyPhone: phoneDigits.length >= 10 ? phoneDigits : undefined,
        addressZipCode: onlyDigits(address?.zip) || undefined,
        addressStreet: address?.street || undefined,
        addressNumber: address?.number || undefined,
        addressDistrict: address?.district || undefined,
        addressCity: address?.city || undefined,
        addressState: address?.state?.toUpperCase() || undefined,
        adminName: admin?.name || undefined,
    };
}

interface UseCnpjAutofillArgs {
    getValues: UseFormGetValues<SignupFormData>;
    setValue: UseFormSetValue<SignupFormData>;
    /** Compartilhado com o autofill do CEP — os dois escrevem nos campos de endereço. */
    tracker: AutofillTracker;
}

export function useCnpjAutofill({ getValues, setValue, tracker }: UseCnpjAutofillArgs) {
    const [status, setStatus] = useState<CnpjAutofillStatus>({ state: "idle" });

    /** Último CNPJ consultado, para não repetir a chamada a cada blur. */
    const lastQueriedRef = useRef<string>("");
    const abortRef = useRef<AbortController | null>(null);

    const runLookup = useCallback(
        async (rawCnpj: string, { force = false }: { force?: boolean } = {}) => {
            const digits = onlyDigits(rawCnpj);

            if (digits.length !== 14) return;
            if (!isValidCnpj(digits)) {
                // O zod já marca o campo; não vale gastar uma requisição.
                setStatus({ state: "idle" });
                return;
            }
            if (!force && digits === lastQueriedRef.current) return;

            abortRef.current?.abort();
            const controller = new AbortController();
            abortRef.current = controller;

            lastQueriedRef.current = digits;
            setStatus({ state: "loading" });

            try {
                const data = await lookupCnpj(digits, controller.signal);
                const patch = buildPatch(data);

                let filledCount = 0;
                let keptCount = 0;

                for (const field of FILLABLE_FIELDS) {
                    const incoming = patch[field];
                    if (!incoming) continue;

                    const current = (getValues(field) as string | undefined) ?? "";

                    // Sobrescreve só o que está vazio ou o que algum autopreenchimento
                    // colocou ali. Nada digitado à mão é perdido.
                    if (tracker.canOverwrite(field, current)) {
                        setValue(field, incoming, { shouldValidate: true, shouldDirty: true });
                        tracker.remember(field, incoming);
                        filledCount++;
                    } else if (current !== incoming) {
                        keptCount++;
                    }
                }

                setStatus({ state: "filled", data, filledCount, keptCount });
            } catch (error) {
                if (controller.signal.aborted) return;
                lastQueriedRef.current = ""; // permite tentar de novo no próximo blur
                setStatus({ state: "error", message: describeCnpjError(error) });
            }
        },
        [getValues, setValue, tracker],
    );

    /** Handler para o onBlur do campo CNPJ. */
    const handleCnpjBlur = useCallback(
        (value: string) => {
            void runLookup(value);
        },
        [runLookup],
    );

    /** Botão "consultar de novo", ignorando o controle de CNPJ repetido. */
    const retryLookup = useCallback(() => {
        void runLookup(getValues("cnpj") ?? "", { force: true });
    }, [getValues, runLookup]);

    return { status, handleCnpjBlur, retryLookup };
}
