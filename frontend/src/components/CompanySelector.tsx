import { useEffect, useState } from "react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/apiClient";
import { useAuthStore } from "../stores/authStore";
import { getAllowedCompanies } from "./CompanyContextGate";
import { Link } from "react-router-dom";

type Branch = { id: number; name: string; isActive: boolean };

export function CompanySelector() {
    const { companyId, branchId, setCompany, setBranchId } = useAuthStore();
    const client = useQueryClient();
    const [switching, setSwitching] = useState(false);
    const [error, setError] = useState("");
    const companies = useQuery({ queryKey: ["allowed-companies", companyId], queryFn: getAllowedCompanies });
    const branches = useQuery({ queryKey: ["company-branches", companyId], queryFn: () => api<Branch[]>(`/api/branches/company/${companyId}`), enabled: !!companyId });
    const selected = companies.data?.find(c => c.companyId === companyId);

    useEffect(() => {
        if (selected) useAuthStore.setState({ businessGroupId: selected.businessGroupId, employeeId: selected.employeeId });
    }, [selected]);
    useEffect(() => {
        if (branches.data && !branches.data.some(b => b.id === branchId)) setBranchId(branches.data[0]?.id ?? 0);
    }, [branches.data, branchId, setBranchId]);

    async function changeCompany(value: number) {
        const company = companies.data?.find(c => c.companyId === value);
        if (!company || value === companyId) return;
        setSwitching(true);
        try {
        const session = await api<{ accessToken: string; employeeId: number | null }>("/api/companies/switch", {
            method: "POST", body: JSON.stringify({ targetCompanyId: value })
        });
        await client.cancelQueries();
        client.clear();
        setCompany(company.companyId, company.businessGroupId);
        useAuthStore.setState({ accessToken: session.accessToken, employeeId: session.employeeId });
        // Discard local forms, drawers and callbacks as well as the query cache.
        window.location.replace("/");
        } catch (cause) {
            setError(cause instanceof Error ? cause.message : "Não foi possível trocar a empresa.");
            setSwitching(false);
        }
    }

    return <div style={{ display: "flex", gap: 8, alignItems: "center", flexWrap: "wrap" }}>
        {selected && <span>{selected.groupName}</span>}
        {selected?.roles.includes("Administrador") && <Link to="/empresas">Gerenciar empresas</Link>}
        <label>Empresa <select aria-label="Empresa" value={companyId ?? ""} disabled={switching || !companies.data} onChange={e => void changeCompany(Number(e.target.value))}>
            {companies.data?.map(c => <option key={c.companyId} value={c.companyId}>{c.tradeName}</option>)}
        </select></label>
        <label>Filial <select aria-label="Filial" value={branchId} disabled={switching || !branches.data?.length} onChange={async e => {
            const id = Number(e.target.value);
            await client.cancelQueries();
            client.clear();
            setBranchId(id);
            window.location.replace("/");
        }}>
            {!branches.data?.length && <option value={0}>Nenhuma filial</option>}
            {branches.data?.filter(b => b.isActive).map(b => <option key={b.id} value={b.id}>{b.name}</option>)}
        </select></label>
        {(companies.isError || branches.isError) && <span role="alert">Não foi possível carregar as empresas e filiais.</span>}
        {error && <span role="alert">{error}</span>}
    </div>;
}
