/**
 * Registro de procedência dos campos preenchidos automaticamente.
 *
 * Dois autopreenchimentos escrevem nos mesmos campos de endereço: o do CNPJ (endereço da
 * empresa na Receita) e o do CEP (endereço dos Correios). Sem um registro comum, cada hook
 * só reconheceria o que ele mesmo escreveu — trocar o CEP depois de consultar o CNPJ não
 * atualizaria nada, porque os campos estariam "cheios".
 *
 * A regra é uma só: um campo pode ser sobrescrito quando está vazio ou quando o valor atual
 * é exatamente o que algum autopreenchimento colocou ali. Qualquer coisa digitada à mão fica.
 */
export interface AutofillTracker {
    canOverwrite(field: string, currentValue: string | undefined | null): boolean;
    remember(field: string, value: string): void;
}

export function createAutofillTracker(): AutofillTracker {
    const written = new Map<string, string>();

    return {
        canOverwrite(field, currentValue) {
            const current = (currentValue ?? "").trim();
            if (current === "") return true;
            return written.get(field) === currentValue;
        },
        remember(field, value) {
            written.set(field, value);
        },
    };
}
