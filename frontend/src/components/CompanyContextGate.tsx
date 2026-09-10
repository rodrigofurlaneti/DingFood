import { useEffect, type ReactNode } from "react";
import { useQuery } from "@tanstack/react-query";
import { useAuthStore } from "../stores/authStore";
import { api } from "../lib/apiClient";

export type AllowedCompany = { companyId: number; businessGroupId: number; groupName: string; tradeName: string; employeeId: number | null; roles: string[] };
export type CompanyBranch = { id: number; name: string; isActive: boolean };
export const getAllowedCompanies = () => {
    const { homeCompanyId, companyId } = useAuthStore.getState();
    return api<AllowedCompany[]>("/api/companies/allowed", { headers: { "X-Company-Id": String(homeCompanyId ?? companyId) } });
};

export function CompanyContextGate({ children }: { children: ReactNode }) {
    const { companyId, branchId, setCompany, setBranchId, clear } = useAuthStore();
    const companies = useQuery({ queryKey: ["allowed-companies", companyId], queryFn: getAllowedCompanies, retry: false });
    const selected = companies.data?.find(c => c.companyId === companyId);
    const branches = useQuery({ queryKey: ["company-branches", companyId], queryFn: () => api<CompanyBranch[]>(`/api/branches/company/${companyId}`), enabled: !!selected, retry: false });
    const activeBranches = branches.data?.filter(b => b.isActive);
    const branchValid = activeBranches?.some(b => b.id === branchId);
    useEffect(() => {
        if (companies.data?.length && !selected) {
            const first = companies.data[0];
            setCompany(first.companyId, first.businessGroupId);
        }
        if (selected) useAuthStore.setState({ businessGroupId: selected.businessGroupId, employeeId: selected.employeeId });
    }, [companies.data, selected, setCompany]);
    useEffect(() => {
        if (selected && activeBranches?.length && !branchValid) setBranchId(activeBranches[0].id);
    }, [selected, activeBranches, branchValid, setBranchId]);
    if (companies.isError || branches.isError || companies.data?.length === 0 || activeBranches?.length === 0)
        return <div role="alert">Não foi possível abrir uma empresa e filial autorizadas. <button onClick={() => window.location.reload()}>Tentar novamente</button> <button onClick={clear}>Sair</button></div>;
    if (!selected || !branchValid) return <div role="status">Carregando empresa e filial…</div>;
    return <>{children}</>;
}
