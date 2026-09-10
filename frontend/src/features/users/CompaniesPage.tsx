import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../../lib/apiClient";
import { getRoles } from "./api";
import { getEmployeesByBranch } from "../employees/api";
import { useAuthStore } from "../../stores/authStore";

export function CompaniesPage() {
    const { companyId, branchId } = useAuthStore();
    const client = useQueryClient();
    const [message, setMessage] = useState("");
    const users = useQuery({ queryKey: ["group-users", companyId], queryFn: () => api<{ id: number; userName: string }[]>("/api/companies/group/users") });
    const roles = useQuery({ queryKey: ["roles", companyId], queryFn: () => getRoles(companyId!) });
    const employees = useQuery({ queryKey: ["employees", branchId], queryFn: () => getEmployeesByBranch(branchId) });
    const create = useMutation({ mutationFn: (payload: object) => api<number>("/api/companies/group", { method: "POST", body: JSON.stringify(payload) }), onSuccess: () => {
        void client.invalidateQueries({ queryKey: ["allowed-companies"] });
        setMessage("Empresa criada. Selecione-a no topo e configure equipe, caixa e integrações.");
    } });
    const access = useMutation({ mutationFn: (payload: object) => api<void>("/api/companies/access", { method: "PUT", body: JSON.stringify(payload) }), onSuccess: () => setMessage("Acesso atualizado para a empresa selecionada.") });
    return <main className="page" style={{ padding: 24, maxWidth: 760 }}>
        <h1>Empresas do grupo</h1>
        <p>Cada empresa mantém seu próprio CNPJ, cardápio, estoque e integrações.</p>
        <h2>Nova empresa</h2>
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
        <h2>Acesso à empresa selecionada</h2>
        <p>Conceda acesso explicitamente. O funcionário e o perfil devem pertencer a esta empresa.</p>
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
        {message && <p role="status">{message}</p>}
        {(create.error || access.error || users.error) && <p role="alert">{(create.error ?? access.error ?? users.error)?.message}</p>}
    </main>;
}
