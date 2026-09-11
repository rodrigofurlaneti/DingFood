import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../../lib/apiClient";
import { getRoles } from "./api";
import { getEmployeesByBranch } from "../employees/api";
import { type CompanyBranch, getAllowedCompanies } from "../../components/CompanyContextGate";
import { useAuthStore } from "../../stores/authStore";

export function CompaniesPage() {
    const { companyId, branchId } = useAuthStore();
    const client = useQueryClient();
    const [message, setMessage] = useState("");
    const [mode, setMode] = useState<"company" | "brand" | "branch">("company");
    const companies = useQuery({ queryKey: ["allowed-companies", companyId], queryFn: getAllowedCompanies });
    const workplaces = useQuery({ queryKey: ["company-branches", companyId], queryFn: () => api<CompanyBranch[]>("/api/workplaces") });
    const brandOptions = useQuery({ queryKey: ["group-brands", companyId], queryFn: () => api<{ id: number; name: string }[]>("/api/workplaces/brands") });
    const workplace = useMutation({ mutationFn: (payload: object) => api<number>("/api/workplaces", { method: "POST", body: JSON.stringify(payload) }), onSuccess: () => {
        void client.invalidateQueries({ queryKey: ["company-branches"] });
        void client.invalidateQueries({ queryKey: ["group-brands"] });
        setMessage("Cadastro concluído. A marca e a filial já estão disponíveis no seletor do topo. Configure a equipe e as integrações nessa filial.");
    } });
    const users = useQuery({ queryKey: ["group-users", companyId], queryFn: () => api<{ id: number; userName: string }[]>("/api/companies/group/users") });
    const roles = useQuery({ queryKey: ["roles", companyId], queryFn: () => getRoles(companyId!) });
    const employees = useQuery({ queryKey: ["employees", branchId], queryFn: () => getEmployeesByBranch(branchId) });
    const create = useMutation({ mutationFn: (payload: object) => api<number>("/api/companies/group", { method: "POST", body: JSON.stringify(payload) }), onSuccess: () => {
        void client.invalidateQueries({ queryKey: ["allowed-companies"] });
        setMessage("Empresa criada. Selecione-a no topo e configure equipe, caixa e integrações.");
    } });
    const access = useMutation({ mutationFn: async (payload: { appUserId: number; roleId: number | null; employeeId: number | null; enabled: boolean }) => {

        await api<void>("/api/workplaces/access", { method: "PUT", body: JSON.stringify({ ...payload, branchId }) });
    }, onSuccess: () => setMessage("Acesso atualizado somente para a filial selecionada.") });
    return <main className="companies-page">
        <h1>Empresas, marcas e filiais</h1>
                <p>Empresa ativa: <strong>{companies.data?.find(c => c.companyId === companyId)?.tradeName}</strong>. Cada marca tem seu catálogo; cada filial mantém sua equipe e operação.</p>
        <nav className="company-create-actions" aria-label="Tipo de cadastro">
            <button type="button" aria-pressed={mode === "company"} onClick={() => { setMode("company"); setMessage(""); }}>+ Nova empresa</button>
            <button type="button" aria-pressed={mode === "brand"} onClick={() => { setMode("brand"); setMessage(""); }}>+ Nova marca</button>
            <button type="button" aria-pressed={mode === "branch"} onClick={() => { setMode("branch"); setMessage(""); }}>+ Nova filial</button>
        </nav>
        <div className="companies-layout"><section className="companies-card">
        {mode === "company" ? <><h2>Nova empresa</h2>
        <p>Cadastre outro CNPJ no grupo. Para uma marca no CNPJ atual, use Nova marca.</p>
        <form style={{ display: "grid", gap: 12 }} onSubmit={e => {
            e.preventDefault();
            const values = Object.fromEntries(new FormData(e.currentTarget));
            create.mutate(values);
        }}>
            <label>Razão social <input name="legalName" required maxLength={200} /></label>
            <label>Nome fantasia <input name="tradeName" required maxLength={150} /></label>
            <label>CNPJ <input name="cnpj" required inputMode="numeric" maxLength={18} /></label>
            <label>Primeira filial <input name="branchName" required maxLength={150} /></label>
            <button disabled={create.isPending}>Criar empresa no grupo</button>
        </form>
        </> : <>
            <h2>{mode === "brand" ? "Nova marca" : "Nova filial"}</h2>
            <p>{mode === "brand" ? "Crie uma marca e sua primeira filial na empresa ativa." : "Abra outra filial para uma marca do grupo, vinculada à empresa ativa."}</p>
            <form key={mode} style={{ display: "grid", gap: 12 }} onSubmit={e => {
                e.preventDefault(); const values = new FormData(e.currentTarget);
                const form = e.currentTarget;
                workplace.mutate({ brandName: values.get("brandName") ?? "", branchName: values.get("branchName"), existingBrandId: Number(values.get("existingBrandId")) || null }, { onSuccess: () => form.reset() });
            }}>
                {mode === "brand" ? <label>Nome da marca<input name="brandName" required maxLength={150} placeholder="Ex.: Burger da Casa" /></label> :
                    <label>Marca<select name="existingBrandId" required defaultValue=""><option value="" disabled>Selecione a marca</option>{brandOptions.data?.map(b => <option key={b.id} value={b.id}>{b.name}</option>)}</select></label>}
                <label>{mode === "brand" ? "Primeira filial da marca" : "Nome da nova filial"}<input name="branchName" required maxLength={150} placeholder="Ex.: Centro" /></label>
                <p>O acesso inicial será seu. Outros usuários precisam de autorização específica para esta filial.</p>
                <button disabled={workplace.isPending || (mode === "branch" && !brandOptions.data?.length)}>{workplace.isPending ? "Salvando…" : mode === "brand" ? "Criar marca" : "Criar filial"}</button>
            </form>
        </>}
        </section><section className="companies-card">
        <h2>Acesso à filial selecionada</h2>
        <p><strong>{workplaces.data?.find(b => b.id === branchId)?.brandName} · {workplaces.data?.find(b => b.id === branchId)?.name}</strong><br />A autorização vale somente para esta filial. Para configurar outra, selecione-a no topo.</p>
        <form style={{ display: "grid", gap: 12 }} onSubmit={e => {
            e.preventDefault();
            const values = new FormData(e.currentTarget);
            access.mutate({ appUserId: Number(values.get("user")), roleId: Number(values.get("role")) || null,
                employeeId: Number(values.get("employee")) || null, enabled: values.get("enabled") === "true" });
        }}>
            <label>Usuário <select name="user" required><option value="">Selecione</option>{users.data?.map(u => <option key={u.id} value={u.id}>{u.userName}</option>)}</select></label>
            <label>Perfil nesta empresa <select name="role"><option value="">Sem perfil administrativo</option>{roles.data?.map(r => <option key={r.id} value={r.id}>{r.name}</option>)}</select></label>
            <label>Funcionário nesta filial <select name="employee"><option value="">Sem vínculo com funcionário</option>{employees.data?.map(r => <option key={r.id} value={r.id}>{r.name}</option>)}</select></label>
            <label>Acesso <select name="enabled"><option value="true">Conceder / atualizar</option><option value="false">Revogar</option></select></label>
            <button disabled={access.isPending || users.isError || !users.data}>Salvar acesso</button>
        </form>
        </section></div>
        {message && <p role="status">{message}</p>}
        {(create.error || workplace.error || access.error || users.error || brandOptions.error) && <p role="alert">{(create.error ?? workplace.error ?? access.error ?? users.error ?? brandOptions.error)?.message}</p>}
    </main>;
}
