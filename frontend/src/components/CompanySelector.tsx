import { useEffect, useRef, useState } from "react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/apiClient";
import { useAuthStore } from "../stores/authStore";
import { getAllowedCompanies, type CompanyBranch } from "./CompanyContextGate";
import { Link } from "react-router-dom";

type Branch = CompanyBranch;

export function CompanySelector() {
    const { companyId, branchId, setCompany, setBranchId } = useAuthStore();
    const client = useQueryClient();
    const [open, setOpen] = useState(false);
    const container = useRef<HTMLDivElement>(null);
    const trigger = useRef<HTMLButtonElement>(null);
    useEffect(() => {
        if (!open) return;
        function dismiss(event: PointerEvent) {
            if (!container.current?.contains(event.target as Node)) setOpen(false);
        }
        function escape(event: KeyboardEvent) {
            if (event.key === "Escape") { setOpen(false); trigger.current?.focus(); }
        }
        document.addEventListener("pointerdown", dismiss);
        document.addEventListener("keydown", escape);
        return () => {
            document.removeEventListener("pointerdown", dismiss);
            document.removeEventListener("keydown", escape);
        };
    }, [open]);
    const [switching, setSwitching] = useState(false);
    const [error, setError] = useState("");
    const companies = useQuery({ queryKey: ["allowed-companies", companyId], queryFn: getAllowedCompanies });
    const branches = useQuery({ queryKey: ["company-branches", companyId], queryFn: () => api<Branch[]>("/api/workplaces"), enabled: !!companyId });
    const selected = companies.data?.find(c => c.companyId === companyId);

    useEffect(() => {
        if (selected) useAuthStore.setState({ businessGroupId: selected.businessGroupId, employeeId: branches.data?.find(b => b.id === branchId)?.employeeId ?? null });
    }, [selected, branches.data, branchId]);
    useEffect(() => {
        if (branches.data && !branches.data.some(b => b.id === branchId)) setBranchId(branches.data[0]?.id ?? 0);
    }, [branches.data, branchId, setBranchId]);

    async function changeCompany(value: number) {
        const company = companies.data?.find(c => c.companyId === value);
        if (!company || value === companyId) return;
        setError("");
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

    const activeBranches = branches.data?.filter(b => b.isActive) ?? [];
    const selectedBranch = activeBranches.find(b => b.id === branchId);
    const branchName = selectedBranch?.name ?? "Nenhuma filial";
    const availableBrands = [...new Map(activeBranches.map(b => [b.brandId, b.brandName])).entries()];
    async function changeBranch(id: number) {
        if (id === branchId || !activeBranches.some(b => b.id === id)) return;
        setSwitching(true);
        await client.cancelQueries();
        client.clear();
        setBranchId(id);
        window.location.replace("/");
    }
    return <div className="company-context" ref={container} onBlur={event => {
        if (!event.currentTarget.contains(event.relatedTarget as Node)) setOpen(false);
    }}>
        <button type="button" className="company-context-trigger" ref={trigger}
            aria-label="Selecionar empresa e filial" aria-expanded={open} aria-controls="company-context-panel"
            onClick={() => setOpen(value => !value)}>
            <span className="company-context-icon" aria-hidden="true">▦</span>
            <span className="company-context-summary">
                <strong title={selected?.tradeName}>{selected?.tradeName ?? "Carregando empresa…"}</strong>
                <span><i aria-hidden="true" />{branches.data?.find(b => b.id === branchId)?.brandName} · {branchName}</span>
            </span>
            <span className="company-context-chevron" aria-hidden="true">⌄</span>
        </button>
        {open && <section id="company-context-panel" className="company-context-panel" aria-label="Empresa e filial">
            <div className="company-context-heading">
                <strong>Local de trabalho</strong>
                <span>{selected?.groupName}</span>
            </div>
            <label>Empresa <select aria-label="Empresa" value={companyId ?? ""} disabled={switching || !companies.data} onChange={e => void changeCompany(Number(e.target.value))}>
                {companies.data?.map(c => <option key={c.companyId} value={c.companyId}>{c.tradeName}</option>)}
            </select></label>
            <label>Marca <select aria-label="Marca" value={selectedBranch?.brandId ?? ""} disabled={switching || !availableBrands.length}
                onChange={e => { const branch = activeBranches.find(b => b.brandId === Number(e.target.value)); if (branch) void changeBranch(branch.id); }}>
                {availableBrands.map(([id, name]) => <option key={id} value={id}>{name}</option>)}
            </select></label>
            <label>Filial <select aria-label="Filial" value={branchId} disabled={switching || !activeBranches.length} onChange={e => void changeBranch(Number(e.target.value))}>
                {!activeBranches.length && <option value={0}>Nenhuma filial</option>}
                {activeBranches.filter(b => b.brandId === selectedBranch?.brandId).map(b => <option key={b.id} value={b.id}>{b.name}</option>)}
            </select></label>            <p className="company-context-hint" role="status">{switching ? "Alterando local de trabalho…" : "O catálogo acompanha a marca. Pedidos, equipe e caixa acompanham a filial."}</p>
            {selected?.roles.includes("Administrador") && <Link className="company-context-manage" to="/empresas" onClick={() => setOpen(false)}>Empresas, marcas e filiais <span aria-hidden="true">→</span></Link>}
        </section>}
        {(companies.isError || branches.isError) && <span role="alert">Não foi possível carregar as empresas e filiais.</span>}
        {error && <span role="alert">{error}</span>}
    </div>;
}