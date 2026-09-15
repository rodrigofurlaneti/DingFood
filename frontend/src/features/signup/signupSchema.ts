import { z } from "zod";

export const signupSchema = z
    .object({
        legalName: z
            .string()
            .min(1, "Razão social é obrigatória")
            .min(3, "Razão social deve ter no mínimo 3 caracteres"),

        tradeName: z
            .string()
            .min(1, "Nome fantasia é obrigatório")
            .min(3, "Nome fantasia deve ter no mínimo 3 caracteres"),

        cnpj: z
            .string()
            .min(1, "CNPJ é obrigatório")
            .refine((val: string) => /^\d{14}$/.test(val.replace(/\D/g, "")),
                "CNPJ deve conter 14 dígitos"),

        branchName: z
            .literal("Matriz", {
                errorMap: () => ({ message: "A filial deve ser 'Matriz'" }),
            }),

        adminName: z
            .string()
            .min(1, "Nome do administrador é obrigatório")
            .min(5, "Nome deve ter no mínimo 5 caracteres"),

        adminCpf: z
            .string()
            .min(1, "CPF do administrador é obrigatório")
            .refine((val: string) => /^\d{11}$/.test(val.replace(/\D/g, "")),
                "CPF deve conter 11 dígitos"),

        adminUserName: z
            .string()
            .min(1, "Usuário é obrigatório")
            .min(3, "Usuário deve ter no mínimo 3 caracteres")
            .max(20, "Usuário deve ter no máximo 20 caracteres")
            .regex(/^[a-zA-Z0-9._-]+$/, "Usuário pode conter apenas letras, números, pontos, hífen e underscore"),

        adminEmail: z
            .string()
            .min(1, "E-mail é obrigatório")
            .email("E-mail inválido"),

        adminPassword: z
            .string()
            .min(1, "Senha é obrigatória")
            .min(8, "Senha deve ter no mínimo 8 caracteres")
            .regex(/[A-Z]/, "Senha deve conter pelo menos uma letra maiúscula")
            .regex(/[a-z]/, "Senha deve conter pelo menos uma letra minúscula")
            .regex(/[0-9]/, "Senha deve conter pelo menos um número"),

        confirmPassword: z
            .string()
            .min(1, "Confirme a senha"),
    })
    .refine((data) => data.adminPassword === data.confirmPassword, {
        message: "As senhas não coincidem",
        path: ["confirmPassword"],
    });

export type SignupFormData = z.infer<typeof signupSchema>;