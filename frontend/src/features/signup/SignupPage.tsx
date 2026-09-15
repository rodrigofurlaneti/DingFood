import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import InputMask from "react-input-mask";
import { toast } from "sonner";
import { signupApi } from "./signupApi";
import { signupSchema, SignupFormData } from "./signupSchema";
import "./SignupPage.css";

function EyeIcon() {
    return (
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8Z" />
            <circle cx="12" cy="12" r="3" />
        </svg>
    );
}

function EyeOffIcon() {
    return (
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <path d="M17.94 17.94A10.94 10.94 0 0 1 12 20c-7 0-11-8-11-8a21.86 21.86 0 0 1 5.06-6.06" />
            <path d="M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a21.86 21.86 0 0 1-3.22 4.53" />
            <path d="M14.12 14.12a3 3 0 1 1-4.24-4.24" />
            <path d="M1 1l22 22" />
        </svg>
    );
}

export function SignupPage() {
    const navigate = useNavigate();
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);

    const {
        register,
        handleSubmit,
        control,
        watch,
        formState: { errors, isSubmitting },
    } = useForm<SignupFormData>({
        resolver: zodResolver(signupSchema),
        mode: "onBlur",
        defaultValues: {
            branchName: "Matriz",
        },
    });

    // Valor atual da senha, lido a cada digitação para o checklist dinâmico
    const passwordValue = watch("adminPassword") || "";
    const passwordChecks = {
        length: passwordValue.length >= 8,
        uppercase: /[A-Z]/.test(passwordValue),
        lowercase: /[a-z]/.test(passwordValue),
        number: /[0-9]/.test(passwordValue),
    };

    const mutation = useMutation({
        mutationFn: (data: SignupFormData) => {
            const cleanData: SignupFormData = {
                ...data,
                cnpj: data.cnpj.replace(/\D/g, ""),
                adminCpf: data.adminCpf.replace(/\D/g, ""),
                companyPhone: data.companyPhone ? data.companyPhone.replace(/\D/g, "") : undefined,
                branchCnpj: data.branchCnpj ? data.branchCnpj.replace(/\D/g, "") : undefined,
                addressZipCode: data.addressZipCode ? data.addressZipCode.replace(/\D/g, "") : undefined,
            };
            return signupApi(cleanData);
        },
        onSuccess: () => {
            toast.success("Conta criada com sucesso!");
            setTimeout(() => {
                navigate("/login", { replace: true });
            }, 1500);
        },
        onError: (error) => {
            const message =
                error instanceof Error
                    ? error.message
                    : "Não foi possível conectar à API.";
            toast.error(message);
        },
    });

    const onSubmit = (data: SignupFormData) => {
        mutation.mutate(data);
    };

    return (
        <div className="signup-container">
            <form className="signup-form" onSubmit={handleSubmit(onSubmit)}>
                {/* ============ HEADER ============ */}
                <div className="signup-header">
                    <div className="brand">
                        Ding<em>Food</em>
                    </div>
                    <div className="subtitle">
                        cadastre seu restaurante e comece a vender online!
                    </div>
                </div>

                {/* ============ SEÇÃO 1: DADOS DA EMPRESA ============ */}
                <section className="form-section">
                    <h2 className="section-title">📋 Dados da Empresa</h2>

                    <div className="form-row">
                        {/* Razão Social */}
                        <div className="form-group">
                            <label htmlFor="legalName" className="form-label">
                                Razão social *
                            </label>
                            <input
                                id="legalName"
                                data-testid="legalName"
                                placeholder="Ex: DingFood Ltda"
                                className={`form-input ${errors.legalName ? "input-error" : ""
                                    }`}
                                {...register("legalName")}
                                autoFocus
                            />
                            {errors.legalName && (
                                <span className="error-message">
                                    {errors.legalName.message}
                                </span>
                            )}
                        </div>

                        {/* Nome Fantasia */}
                        <div className="form-group">
                            <label htmlFor="tradeName" className="form-label">
                                Nome fantasia *
                            </label>
                            <input
                                id="tradeName"
                                data-testid="tradeName"
                                placeholder="Ex: Seu Restaurante"
                                className={`form-input ${errors.tradeName ? "input-error" : ""
                                    }`}
                                {...register("tradeName")}
                            />
                            {errors.tradeName && (
                                <span className="error-message">
                                    {errors.tradeName.message}
                                </span>
                            )}
                        </div>
                    </div>

                    <div className="form-row">
                        {/* CNPJ com Mask */}
                        <div className="form-group">
                            <label htmlFor="cnpj" className="form-label">
                                CNPJ *
                            </label>
                            <Controller
                                name="cnpj"
                                control={control}
                                render={({ field }: any) => (
                                    <InputMask
                                        mask="99.999.999/0000-99"
                                        {...field}
                                        placeholder="00.000.000/0000-00"
                                    >
                                        {(inputProps: any) => (
                                            <input
                                                {...inputProps}
                                                id="cnpj"
                                                data-testid="cnpj"
                                                type="text"
                                                className={`form-input ${errors.cnpj
                                                    ? "input-error"
                                                    : ""
                                                    }`}
                                            />
                                        )}
                                    </InputMask>
                                )}
                            />
                            {errors.cnpj && (
                                <span className="error-message">
                                    {errors.cnpj.message}
                                </span>
                            )}
                        </div>

                        {/* Primeira Filial (Desabilitado) */}
                        <div className="form-group">
                            <label htmlFor="branchName" className="form-label">
                                Primeira filial *
                            </label>
                            <input
                                id="branchName"
                                data-testid="branchName"
                                type="text"
                                value="Matriz"
                                disabled
                                className="form-input form-input-disabled"
                                title="A primeira filial é automaticamente 'Matriz'"
                            />
                            <span className="helper-text">
                                Automaticamente definido como Matriz
                            </span>
                        </div>
                    </div>

                    <div className="form-row">
                        {/* Telefone da Empresa (opcional) */}
                        <div className="form-group">
                            <label htmlFor="companyPhone" className="form-label">
                                Telefone da empresa <span className="optional-tag">(opcional)</span>
                            </label>
                            <Controller
                                name="companyPhone"
                                control={control}
                                render={({ field }: any) => (
                                    <InputMask
                                        mask="(99) 99999-9999"
                                        {...field}
                                        placeholder="(00) 00000-0000"
                                    >
                                        {(inputProps: any) => (
                                            <input
                                                {...inputProps}
                                                id="companyPhone"
                                                data-testid="companyPhone"
                                                type="text"
                                                className={`form-input ${errors.companyPhone ? "input-error" : ""
                                                    }`}
                                            />
                                        )}
                                    </InputMask>
                                )}
                            />
                            {errors.companyPhone && (
                                <span className="error-message">
                                    {errors.companyPhone.message}
                                </span>
                            )}
                        </div>

                        {/* CNPJ da Filial (opcional) */}
                        <div className="form-group">
                            <label htmlFor="branchCnpj" className="form-label">
                                CNPJ da filial <span className="optional-tag">(opcional)</span>
                            </label>
                            <Controller
                                name="branchCnpj"
                                control={control}
                                render={({ field }: any) => (
                                    <InputMask
                                        mask="99.999.999/0000-99"
                                        {...field}
                                        placeholder="00.000.000/0000-00"
                                    >
                                        {(inputProps: any) => (
                                            <input
                                                {...inputProps}
                                                id="branchCnpj"
                                                data-testid="branchCnpj"
                                                type="text"
                                                className={`form-input ${errors.branchCnpj ? "input-error" : ""
                                                    }`}
                                            />
                                        )}
                                    </InputMask>
                                )}
                            />
                            {errors.branchCnpj && (
                                <span className="error-message">
                                    {errors.branchCnpj.message}
                                </span>
                            )}
                            <span className="helper-text">
                                Deixe em branco se for igual ao CNPJ da empresa
                            </span>
                        </div>
                    </div>
                </section>

                <hr className="form-divider" />

                {/* ============ SEÇÃO 2: ENDEREÇO (OPCIONAL) ============ */}
                <section className="form-section">
                    <h2 className="section-title">📍 Endereço <span className="optional-tag">(opcional)</span></h2>

                    <div className="form-row">
                        {/* CEP */}
                        <div className="form-group">
                            <label htmlFor="addressZipCode" className="form-label">
                                CEP
                            </label>
                            <Controller
                                name="addressZipCode"
                                control={control}
                                render={({ field }: any) => (
                                    <InputMask
                                        mask="99999-999"
                                        {...field}
                                        placeholder="00000-000"
                                    >
                                        {(inputProps: any) => (
                                            <input
                                                {...inputProps}
                                                id="addressZipCode"
                                                data-testid="addressZipCode"
                                                type="text"
                                                className={`form-input ${errors.addressZipCode ? "input-error" : ""
                                                    }`}
                                            />
                                        )}
                                    </InputMask>
                                )}
                            />
                            {errors.addressZipCode && (
                                <span className="error-message">
                                    {errors.addressZipCode.message}
                                </span>
                            )}
                        </div>

                        {/* Cidade */}
                        <div className="form-group">
                            <label htmlFor="addressCity" className="form-label">
                                Cidade
                            </label>
                            <input
                                id="addressCity"
                                data-testid="addressCity"
                                placeholder="Ex: São Paulo"
                                className="form-input"
                                {...register("addressCity")}
                            />
                        </div>
                    </div>

                    <div className="form-row">
                        {/* Rua */}
                        <div className="form-group">
                            <label htmlFor="addressStreet" className="form-label">
                                Rua
                            </label>
                            <input
                                id="addressStreet"
                                data-testid="addressStreet"
                                placeholder="Ex: Av. Paulista"
                                className="form-input"
                                {...register("addressStreet")}
                            />
                        </div>

                        {/* Número */}
                        <div className="form-group">
                            <label htmlFor="addressNumber" className="form-label">
                                Número
                            </label>
                            <input
                                id="addressNumber"
                                data-testid="addressNumber"
                                placeholder="Ex: 1000"
                                className="form-input"
                                {...register("addressNumber")}
                            />
                        </div>
                    </div>

                    <div className="form-row">
                        {/* Bairro */}
                        <div className="form-group">
                            <label htmlFor="addressDistrict" className="form-label">
                                Bairro
                            </label>
                            <input
                                id="addressDistrict"
                                data-testid="addressDistrict"
                                placeholder="Ex: Centro"
                                className="form-input"
                                {...register("addressDistrict")}
                            />
                        </div>

                        {/* Estado (UF) */}
                        <div className="form-group">
                            <label htmlFor="addressState" className="form-label">
                                Estado (UF)
                            </label>
                            <input
                                id="addressState"
                                data-testid="addressState"
                                placeholder="Ex: SP"
                                maxLength={2}
                                className={`form-input ${errors.addressState ? "input-error" : ""}`}
                                style={{ textTransform: "uppercase" }}
                                {...register("addressState")}
                            />
                            {errors.addressState && (
                                <span className="error-message">
                                    {errors.addressState.message}
                                </span>
                            )}
                        </div>
                    </div>
                </section>

                <hr className="form-divider" />

                {/* ============ SEÇÃO 3: DADOS DO ADMINISTRADOR ============ */}
                <section className="form-section">
                    <h2 className="section-title">👤 Administrador</h2>

                    <div className="form-row">
                        {/* Nome Completo */}
                        <div className="form-group">
                            <label htmlFor="adminName" className="form-label">
                                Nome completo *
                            </label>
                            <input
                                id="adminName"
                                data-testid="adminName"
                                placeholder="Ex: João Silva Santos"
                                className={`form-input ${errors.adminName ? "input-error" : ""
                                    }`}
                                {...register("adminName")}
                            />
                            {errors.adminName && (
                                <span className="error-message">
                                    {errors.adminName.message}
                                </span>
                            )}
                        </div>

                        {/* CPF do Administrador com Mask */}
                        <div className="form-group">
                            <label htmlFor="adminCpf" className="form-label">
                                CPF do administrador *
                            </label>
                            <Controller
                                name="adminCpf"
                                control={control}
                                render={({ field }: any) => (
                                    <InputMask
                                        mask="999.999.999-99"
                                        {...field}
                                        placeholder="000.000.000-00"
                                    >
                                        {(inputProps: any) => (
                                            <input
                                                {...inputProps}
                                                id="adminCpf"
                                                data-testid="adminCpf"
                                                type="text"
                                                className={`form-input ${errors.adminCpf
                                                    ? "input-error"
                                                    : ""
                                                    }`}
                                            />
                                        )}
                                    </InputMask>
                                )}
                            />
                            {errors.adminCpf && (
                                <span className="error-message">
                                    {errors.adminCpf.message}
                                </span>
                            )}
                        </div>
                    </div>
                </section>

                <hr className="form-divider" />

                {/* ============ SEÇÃO 4: CREDENCIAIS DE ACESSO ============ */}
                <section className="form-section">
                    <h2 className="section-title">🔐 Credenciais de Acesso</h2>

                    {/* Usuário */}
                    <div className="form-group">
                        <label htmlFor="adminUserName" className="form-label">
                            Usuário administrador *
                        </label>
                        <input
                            id="adminUserName"
                            data-testid="adminUserName"
                            placeholder="Ex: joao.silva"
                            autoComplete="username"
                            className={`form-input ${errors.adminUserName ? "input-error" : ""
                                }`}
                            {...register("adminUserName")}
                        />
                        {errors.adminUserName && (
                            <span className="error-message">
                                {errors.adminUserName.message}
                            </span>
                        )}
                        <span className="helper-text">
                            Letras, números, ponto, hífen e underscore
                        </span>
                    </div>

                    {/* E-mail */}
                    <div className="form-group">
                        <label htmlFor="adminEmail" className="form-label">
                            E-mail do administrador *
                        </label>
                        <input
                            id="adminEmail"
                            data-testid="adminEmail"
                            type="email"
                            placeholder="joao@exemplo.com"
                            autoComplete="email"
                            className={`form-input ${errors.adminEmail ? "input-error" : ""
                                }`}
                            {...register("adminEmail")}
                        />
                        {errors.adminEmail && (
                            <span className="error-message">
                                {errors.adminEmail.message}
                            </span>
                        )}
                        <span className="helper-text">
                            Também será usado como e-mail de contato da empresa
                        </span>
                    </div>

                    {/* Senha */}
                    <div className="form-group">
                        <label htmlFor="adminPassword" className="form-label">
                            Senha *
                        </label>
                        <div className="password-field-wrapper">
                            <input
                                id="adminPassword"
                                data-testid="adminPassword"
                                type={showPassword ? "text" : "password"}
                                placeholder="••••••••"
                                autoComplete="new-password"
                                className={`form-input form-input-password ${errors.adminPassword ? "input-error" : ""
                                    }`}
                                {...register("adminPassword")}
                            />
                            <button
                                type="button"
                                className="password-toggle-btn"
                                onClick={() => setShowPassword((v) => !v)}
                                aria-label={showPassword ? "Ocultar senha" : "Mostrar senha"}
                                tabIndex={-1}
                            >
                                {showPassword ? <EyeOffIcon /> : <EyeIcon />}
                            </button>
                        </div>
                        {errors.adminPassword && (
                            <span className="error-message">
                                {errors.adminPassword.message}
                            </span>
                        )}
                        <div className="password-requirements">
                            <span className={`requirement ${passwordChecks.length ? "requirement-met" : "requirement-unmet"}`}>
                                {passwordChecks.length ? "✓" : "✗"} Mínimo 8 caracteres
                            </span>
                            <span className={`requirement ${passwordChecks.uppercase ? "requirement-met" : "requirement-unmet"}`}>
                                {passwordChecks.uppercase ? "✓" : "✗"} 1 letra maiúscula
                            </span>
                            <span className={`requirement ${passwordChecks.lowercase ? "requirement-met" : "requirement-unmet"}`}>
                                {passwordChecks.lowercase ? "✓" : "✗"} 1 letra minúscula
                            </span>
                            <span className={`requirement ${passwordChecks.number ? "requirement-met" : "requirement-unmet"}`}>
                                {passwordChecks.number ? "✓" : "✗"} 1 número
                            </span>
                        </div>
                    </div>

                    {/* Confirmar Senha */}
                    <div className="form-group">
                        <label htmlFor="confirmPassword" className="form-label">
                            Confirmar senha *
                        </label>
                        <div className="password-field-wrapper">
                            <input
                                id="confirmPassword"
                                data-testid="confirmPassword"
                                type={showConfirmPassword ? "text" : "password"}
                                placeholder="••••••••"
                                autoComplete="new-password"
                                className={`form-input form-input-password ${errors.confirmPassword ? "input-error" : ""
                                    }`}
                                {...register("confirmPassword")}
                            />
                            <button
                                type="button"
                                className="password-toggle-btn"
                                onClick={() => setShowConfirmPassword((v) => !v)}
                                aria-label={showConfirmPassword ? "Ocultar senha" : "Mostrar senha"}
                                tabIndex={-1}
                            >
                                {showConfirmPassword ? <EyeOffIcon /> : <EyeIcon />}
                            </button>
                        </div>
                        {errors.confirmPassword && (
                            <span className="error-message">
                                {errors.confirmPassword.message}
                            </span>
                        )}
                    </div>
                </section>

                {/* ============ BOTÕES DE AÇÃO ============ */}
                <button
                    type="submit"
                    className="btn-primary"
                    disabled={isSubmitting || mutation.isPending}
                    data-testid="submit-signup"
                >
                    {isSubmitting || mutation.isPending
                        ? "Criando conta…"
                        : "Criar minha conta"}
                </button>

                <Link to="/login" className="signup-link">
                    Já tenho conta — entrar
                </Link>

                {/* Asterisco indicando campos obrigatórios */}
                <p className="required-note">* Campos obrigatórios</p>
            </form>
        </div>
    );
}
