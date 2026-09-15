declare module "react-input-mask" {
    import React from "react";

    export interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
        mask?: string | (string | RegExp)[];
        maskChar?: string | null;
        formatChars?: Record<string, string>;
        alwaysShowMask?: boolean;
        beforeMaskedValueChange?: (
            newState: { value: string; selection: { start: number; end: number } | null },
            oldState: { value: string; selection: { start: number; end: number } | null },
            userInput: string,
            maskOptions: unknown
        ) => { value: string; selection: { start: number; end: number } | null };
    }

    const InputMask: React.FC<InputProps>;
    export default InputMask;
}
