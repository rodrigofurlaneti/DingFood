import { useCallback, useRef, useState } from "react";
import type { UseFormGetValues, UseFormSetFocus, UseFormSetValue } from "react-hook-form";
import type { SignupFormData } from "./signupSchema";
import { CepLookupResponse, describeCepError, lookupCep } from "./cepApi";
import { isValidCep } from "./cepUtils";
import { onlyDigits } from "./cnpjUtils";
import type { AutofillTracker } from "./autofillTracker";

export type CepAutofillStatus =
    | { state: "idle" }
    | { state: "loading" }
    | { state: "filled"; data: CepLookupResponse; filledCount: number; keptCount: number }
    | { state: "error"; message: string };

/** Campos do endereço que a consulta de CEP consegue preencher. */
type FillableField = Extract<
    keyof SignupFormData,
    "addressStreet" | "addressDistrict" | "addressCity" | "addressState"
>;

const FILLABLE_FIELDS: FillableField[] = [
    "addressStreet",
    "addressDistrict",
    "addressCity",
    "addressState",
];

function buildPatch(data: CepLookupResponse): Partial<Record<FillableField, string>> {
    return {
        addressStreet: data.street || undefined,
        addressDistrict: data.district || undefined,
        addressCity: data.city || undefined,
        addressState: data.stateAbbreviation?.toUpperCase() || undefined,
    };
}

interface UseCepAutofillArgs {
    getValues: UseFormGetValues<SignupFormData>;
    setValue: UseFormSetValue<SignupFormData>;
    setFocus: UseFormSetFocus<SignupFormData>;
    /** Compartilhado com o autofill do CNPJ — os dois escrevem no endereço. */
    tracker: AutofillTracker;
}

export function useCepAutofill({ getValues, setValue, setFocus, tracker }: UseCepAutofillArgs) {
    const [status, setStatus] = useState<CepAutofillStatus>({ state: "idle" });

    const lastQueriedRef = useRef<string>("");
    const abortRef = useRef<AbortController | null>(null);

    const runLookup = useCallback(
        async (rawCep: string, { force = false }: { force?: boolean } = {}) => {
            const digits = onlyDigits(rawCep);

            if (digits.length !== 8) return;
            if (!isValidCep(digits)) {
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
                const data = await lookupCep(digits, controller.signal);
                const patch = buildPatch(data);

                let filledCount = 0;
                let keptCount = 0;

                for (const field of FILLABLE_FIELDS) {
                    const incoming = patch[field];
                    if (!incoming) continue;

                    const current = (getValues(field) as string | undefined) ?? "";

                    if (tracker.canOverwrite(field, current)) {
                        setValue(field, incoming, { shouldValidate: true, shouldDirty: true });
                        tracker.remember(field, incoming);
                        filledCount++;
                    } else if (current !== incoming) {
                        keptCount++;
                    }
                }

                setStatus({ state: "filled", data, filledCount, keptCount });

                // Padrão clássico de formulário com CEP: o número é a única coisa que o
                // serviço não sabe, então o cursor vai direto para lá.
                if (filledCount > 0 && !((getValues("addressNumber") as string | undefined) ?? "").trim()) {
                    setFocus("addressNumber");
                }
            } catch (error) {
                if (controller.signal.aborted) return;
                lastQueriedRef.current = ""; // permite tentar de novo no próximo blur
                setStatus({ state: "error", message: describeCepError(error) });
            }
        },
        [getValues, setValue, setFocus, tracker],
    );

    /** Handler para o onBlur do campo CEP. */
    const handleCepBlur = useCallback(
        (value: string) => {
            void runLookup(value);
        },
        [runLookup],
    );

    /** Botão "consultar de novo", ignorando o controle de CEP repetido. */
    const retryLookup = useCallback(() => {
        void runLookup(getValues("addressZipCode") ?? "", { force: true });
    }, [getValues, runLookup]);

    return { status, handleCepBlur, retryLookup };
}
