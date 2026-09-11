import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../../lib/apiClient";
import { useAuthStore } from "../../stores/authStore";
import { Link } from "react-router-dom";
import { useToast } from "../../ui/Toast";
import "./own-delivery.css";

type DeliveryTab = "drivers" | "config" | "orders" | "payments";
const DELIVERY_TABS: { id: DeliveryTab; label: string; icon: string }[] = [
    { id: "drivers", label: "Motoboys", icon: "🛵" },
    { id: "config", label: "Modelo de pagamento", icon: "⚙️" },
    { id: "orders", label: "Entregas", icon: "📦" },
    { id: "payments", label: "Diárias a pagar", icon: "💰" },
];

type Driver = { id: number; name: string; phone: string; vehiclePlate: string; employmentType: string; isActive: boolean };
type Condition = { daysOfWeek: number; startMinute: number; endMinute: number; pricePerKm: number; priority: number };
type Config = { model: string; dailyAmount: number; maxRadiusKm: number; pricePerKm: number; timeZoneId: string; conditions: Condition[] };
type Order = { id: number; customerName: string; deliveryDriverId: number | null; deliveryFeeAmount: number; deliveryDistanceKm: number; deliveryPaymentModel: string };
type Payment = { id: number; deliveryDriverId: number; workDate: string; amount: number };
const money = (value: number) => value.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
const defaults: Config = { model: "PerKm", dailyAmount: 0, maxRadiusKm: 10, pricePerKm: 6.99, timeZoneId: "America/Sao_Paulo", conditions: [] };
const blankDriver = { name: "", phone: "", vehiclePlate: "", employmentType: "Freelancer", isActive: true };
const time = (minute: number) => `${String(Math.floor(minute / 60)).padStart(2, "0")}:${String(minute % 60).padStart(2, "0")}`;
const minutes = (value: string) => { const [h, m] = value.split(":").map(Number); return h * 60 + m; };

export function OwnDeliveryPage() {
    const branchId = useAuthStore(s => s.branchId);
    const companyId = useAuthStore(s => s.companyId);
    return <DeliveryWorkspace key={`${companyId}-${branchId}`} />;
}
function DeliveryWorkspace() {
    const toast = useToast();
    const [tab, setTab] = useState<DeliveryTab>("drivers");
    const branchId = useAuthStore(s => s.branchId);
    const queryClient = useQueryClient();
    const key = ["own-delivery", branchId];
    const drivers = useQuery({ queryKey: [...key, "drivers"], queryFn: () => api<Driver[]>("/api/delivery/drivers"), enabled: !!branchId });
    const config = useQuery({ queryKey: [...key, "config"], queryFn: () => api<Config | null>("/api/delivery/config"), enabled: !!branchId });
    const orders = useQuery({ queryKey: [...key, "orders"], queryFn: () => api<Order[]>("/api/delivery/orders"), enabled: !!branchId });
    const payments = useQuery({ queryKey: [...key, "payments"], queryFn: () => api<Payment[]>("/api/delivery/daily-payments"), enabled: !!branchId });
    const [draft, setDraft] = useState<Config | null>(null);
    const [driver, setDriver] = useState(blankDriver);
    const [editing, setEditing] = useState<number | null>(null);
    const [dailyDriver, setDailyDriver] = useState("");
    const [workDate, setWorkDate] = useState(new Date().toLocaleDateString("en-CA"));
    const value = draft ?? config.data ?? defaults;
    const mutation = useMutation({
        mutationFn: ({ path, method, body }: { path: string; method: string; body: unknown }) => api(`/api/delivery/${path}`, { method, body: JSON.stringify(body) }),
        onSuccess: async (_, { path }) => {
            const message = path === "drivers" ? "Motoboy cadastrado."
                : path.startsWith("drivers/") ? "Motoboy atualizado."
                : path === "config" ? "Modelo de pagamento salvo."
                : path === "daily-payments" ? "Diária registrada."
                : "Motoboy atribuído ao pedido.";
            toast.success(message);
            await queryClient.invalidateQueries({ queryKey: key });
        },
        onError: (e: Error) => toast.error(e.message),
    });
    const save = (path: string, method: string, body: unknown) => mutation.mutate({ path, method, body });
    const changeCondition = (index: number, patch: Partial<Condition>) => setDraft({ ...value, conditions: value.conditions.map((c, i) => i === index ? { ...c, ...patch } : c) });
    const error = drivers.error ?? config.error ?? orders.error ?? payments.error;
    if (!branchId) return <p>Selecione uma filial para gerenciar a logística.</p>;
    return <main className="own-delivery">
        <style>{`.own-delivery{max-width:1100px;margin:0 auto;padding:24px;display:grid;gap:24px}.own-delivery section{padding:24px;border:1px solid var(--border,#ddd);border-radius:12px}.own-delivery form,.own-delivery .fields{display:flex;gap:16px;flex-wrap:wrap;align-items:end}.own-delivery label{display:grid;gap:6px}.own-delivery input,.own-delivery select,.own-delivery button{padding:10px;border:1px solid #aaa;border-radius:6px;background:var(--surface,#fff);color:var(--text,#222)}.own-delivery button{cursor:pointer}.own-delivery table{width:100%;border-collapse:collapse}.own-delivery td,.own-delivery th{padding:12px;text-align:left;border-bottom:1px solid #ddd}.own-delivery .scroll{overflow:auto}.own-delivery fieldset{margin:16px 0;border:1px solid #aaa}.own-delivery .days{display:flex;gap:12px;flex-wrap:wrap}.own-delivery .days label{display:flex;align-items:center}.own-delivery button:disabled{opacity:.5;cursor:wait}`}</style>
        <style>{`.own-delivery{box-sizing:border-box;width:100%;min-width:0}.own-delivery section,.own-delivery form,.own-delivery fieldset,.own-delivery .fields,.own-delivery label{min-width:0;max-width:100%}.own-delivery input,.own-delivery select{box-sizing:border-box;max-width:100%}@media(max-width:600px){.own-delivery{padding:12px}.own-delivery section{padding:14px}.own-delivery h1{font-size:1.6rem}.own-delivery .fields>label{width:100%}.own-delivery .days label{width:auto}}`}</style>
        <style>{`.own-delivery section{background:var(--bg-raise);border-color:var(--line)}.own-delivery input,.own-delivery select,.own-delivery button{background:var(--bg-press);color:var(--ink);border-color:var(--line)}.own-delivery a{color:var(--amber)}.own-delivery th,.own-delivery td,.own-delivery fieldset{border-color:var(--line)}.own-delivery h1{font-size:2rem}.own-delivery h2{margin-bottom:16px}.own-delivery p{color:var(--ink-dim);margin:12px 0}.own-delivery form>button:not([type=button]){background:var(--amber);color:var(--amber-ink);font-weight:600}.own-delivery input[type=checkbox]{accent-color:var(--amber)}`}</style>
        <header className="rise"><h1 className="display">Motoboys e taxas de entrega</h1><p>Cadastro de entregadores, tarifas e diárias — configurações exclusivas da filial e marca selecionadas.</p><Link to="/delivery">Voltar ao painel de entregas</Link></header>
        <div role="tablist" aria-label="Áreas de motoboys e taxas" className="delivery-tabs ui-row ui-row-wrap">
            {DELIVERY_TABS.map((item, index) => <button
                key={item.id} type="button" role="tab" id={`delivery-tab-${item.id}`}
                aria-selected={tab === item.id} aria-controls={`delivery-panel-${item.id}`}
                tabIndex={tab === item.id ? 0 : -1} onClick={() => setTab(item.id)}
                onKeyDown={event => {
                    let next: number;
                    if (event.key === "ArrowRight") next = (index + 1) % DELIVERY_TABS.length;
                    else if (event.key === "ArrowLeft") next = (index + DELIVERY_TABS.length - 1) % DELIVERY_TABS.length;
                    else if (event.key === "Home") next = 0;
                    else if (event.key === "End") next = DELIVERY_TABS.length - 1;
                    else return;
                    event.preventDefault(); setTab(DELIVERY_TABS[next].id);
                    document.getElementById(`delivery-tab-${DELIVERY_TABS[next].id}`)?.focus();
                }}>
                <span aria-hidden="true">{item.icon}</span>{item.label}
            </button>)}
        </div>
        {error && <p role="alert">{error.message}</p>}
        {(drivers.isLoading || config.isLoading) && <p>Carregando logística…</p>}
        <section hidden={tab !== "drivers"} role="tabpanel" id="delivery-panel-drivers" aria-labelledby="delivery-tab-drivers" className="rise rise-1"><h2 className="display">{editing ? "Editar motoboy" : "Cadastrar motoboy"}</h2>
            <form onSubmit={e => { e.preventDefault(); mutation.mutate({ path: editing ? `drivers/${editing}` : "drivers", method: editing ? "PUT" : "POST", body: driver }, { onSuccess: () => { setDriver(blankDriver); setEditing(null); } }); }}>
                <label>Nome<input required maxLength={150} value={driver.name} onChange={e => setDriver({ ...driver, name: e.target.value })} /></label>
                <label>Telefone<input required type="tel" maxLength={20} value={driver.phone} onChange={e => setDriver({ ...driver, phone: e.target.value })} /></label>
                <label>Placa<input required maxLength={10} value={driver.vehiclePlate} onChange={e => setDriver({ ...driver, vehiclePlate: e.target.value })} /></label>
                <label>Vínculo<select value={driver.employmentType} onChange={e => setDriver({ ...driver, employmentType: e.target.value })}><option>Freelancer</option><option>Fixo</option></select></label>
                {editing && <label>Ativo<input type="checkbox" checked={driver.isActive} onChange={e => setDriver({ ...driver, isActive: e.target.checked })} /></label>}
                <button disabled={mutation.isPending}>Salvar motoboy</button>{editing && <button type="button" onClick={() => { setEditing(null); setDriver(blankDriver); }}>Cancelar</button>}
            </form>
            <div className="scroll"><table><thead><tr><th>Nome</th><th>Telefone</th><th>Placa</th><th>Vínculo</th><th>Ação</th></tr></thead><tbody>{drivers.data?.map(d => <tr key={d.id}><td>{d.name}{!d.isActive && " (inativo)"}</td><td>{d.phone}</td><td>{d.vehiclePlate}</td><td>{d.employmentType}</td><td><button onClick={() => { setEditing(d.id); setDriver(d); }}>Editar</button></td></tr>)}</tbody></table></div>
            {!drivers.data?.length && <p>Nenhum motoboy cadastrado.</p>}
        </section>
        <section hidden={tab !== "config"} role="tabpanel" id="delivery-panel-config" aria-labelledby="delivery-tab-config" className="rise rise-1"><h2 className="display">Modelo de pagamento</h2><p>A taxa por KM é cobrada no pedido e registrada como custo por entrega. A diária é paga uma vez por motoboy/data e não adiciona taxa ao cliente.</p>
            <form onSubmit={e => { e.preventDefault(); save("config", "PUT", value); }}>
                <div className="fields">
                    <label>Modelo<select value={value.model} onChange={e => setDraft({ ...value, model: e.target.value })}><option value="PerKm">Por entrega / raio KM</option><option value="Daily">Diária fixa</option></select></label>
                    <label>Fuso horário<input required value={value.timeZoneId} onChange={e => setDraft({ ...value, timeZoneId: e.target.value })} /></label>
                    {value.model === "Daily" ? <label>Diária (R$)<input required type="number" min="0" max="100000" step="0.01" value={value.dailyAmount} onChange={e => setDraft({ ...value, dailyAmount: +e.target.value })} /></label> : <>
                        <label>Raio máximo (KM)<input required type="number" min="0.001" max="1000" step="0.001" value={value.maxRadiusKm} onChange={e => setDraft({ ...value, maxRadiusKm: +e.target.value })} /></label>
                        <label>Valor por KM (R$)<input required type="number" min="0" max="100000" step="0.01" value={value.pricePerKm} onChange={e => setDraft({ ...value, pricePerKm: +e.target.value })} /></label>
                    </>}
                </div>
                {value.model === "PerKm" && <div style={{ width: "100%" }}><h3>Tarifas dinâmicas</h3><p>Distância em linha reta. Fora do raio, o pedido é bloqueado. Início inclusivo, fim exclusivo; maior prioridade prevalece. Após meia-noite, vale o dia de início da regra.</p>
                    {value.conditions.map((c, i) => <fieldset key={i}><legend>Regra {i + 1}</legend><div className="days">{["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"].map((day, d) => <label key={day}><input type="checkbox" checked={!!(c.daysOfWeek & (1 << d))} onChange={() => changeCondition(i, { daysOfWeek: c.daysOfWeek ^ (1 << d) })} />{day}</label>)}</div>
                        <div className="fields"><label>Início<input required type="time" value={time(c.startMinute)} onChange={e => changeCondition(i, { startMinute: minutes(e.target.value) })} /></label><label>Fim<input required type="time" value={time(c.endMinute)} onChange={e => changeCondition(i, { endMinute: minutes(e.target.value) })} /></label><label>R$/KM<input required type="number" min="0" max="100000" step="0.01" value={c.pricePerKm} onChange={e => changeCondition(i, { pricePerKm: +e.target.value })} /></label><label>Prioridade<input required type="number" step="1" value={c.priority} onChange={e => changeCondition(i, { priority: +e.target.value })} /></label><button type="button" onClick={() => setDraft({ ...value, conditions: value.conditions.filter((_, j) => i !== j) })}>Remover regra</button></div>
                    </fieldset>)}<button type="button" onClick={() => setDraft({ ...value, conditions: [...value.conditions, { daysOfWeek: 97, startMinute: 1080, endMinute: 1380, pricePerKm: 8.5, priority: Math.max(0, ...value.conditions.map(c => c.priority)) + 1 }] })}>Adicionar tarifa dinâmica</button>
                </div>}
                <button disabled={mutation.isPending || config.isLoading}>Salvar configuração</button>
            </form>
        </section>
        <section hidden={tab !== "orders"} role="tabpanel" id="delivery-panel-orders" aria-labelledby="delivery-tab-orders" className="rise rise-1"><h2 className="display">Atribuir motoboy / custos por pedido</h2><p>Últimos 200 pedidos com precificação própria. Os valores preservam a configuração da criação do pedido.</p><div className="scroll"><table><thead><tr><th>Pedido</th><th>Cliente</th><th>KM</th><th>Taxa / custo</th><th>Motoboy</th></tr></thead><tbody>{orders.data?.map(o => <tr key={o.id}><td>#{o.id}</td><td>{o.customerName}</td><td>{o.deliveryDistanceKm?.toFixed(3)}</td><td>{o.deliveryPaymentModel === "Daily" ? "Diária" : money(o.deliveryFeeAmount)}</td><td>{o.deliveryDriverId ? drivers.data?.find(d => d.id === o.deliveryDriverId)?.name ?? o.deliveryDriverId : <select aria-label={`Motoboy do pedido ${o.id}`} value="" disabled={mutation.isPending} onChange={e => save(`orders/${o.id}/driver`, "PUT", { driverId: +e.target.value })}><option value="">Atribuir motoboy</option>{drivers.data?.filter(d => d.isActive).map(d => <option key={d.id} value={d.id}>{d.name}</option>)}</select>}</td></tr>)}</tbody></table></div>{!orders.data?.length && <p>Nenhum pedido com precificação própria.</p>}</section>
        <section hidden={tab !== "payments"} role="tabpanel" id="delivery-panel-payments" aria-labelledby="delivery-tab-payments" className="rise rise-1"><h2 className="display">Diárias a pagar</h2><p>Registre a diária mesmo quando não houver entregas. A atribuição de pedidos no modelo diária também registra o pagamento, sem duplicar a data.</p>
            <form onSubmit={e => { e.preventDefault(); save("daily-payments", "POST", { driverId: +dailyDriver, workDate }); }}><label>Motoboy<select required value={dailyDriver} onChange={e => setDailyDriver(e.target.value)}><option value="">Selecione</option>{drivers.data?.filter(d => d.isActive).map(d => <option key={d.id} value={d.id}>{d.name}</option>)}</select></label><label>Data<input required type="date" value={workDate} onChange={e => setWorkDate(e.target.value)} /></label><button disabled={mutation.isPending || config.data?.model !== "Daily"}>Registrar diária</button></form>
            <div className="scroll"><table><thead><tr><th>Data</th><th>Motoboy</th><th>Valor a pagar</th></tr></thead><tbody>{payments.data?.map(p => <tr key={p.id}><td>{p.workDate}</td><td>{drivers.data?.find(d => d.id === p.deliveryDriverId)?.name ?? p.deliveryDriverId}</td><td>{money(p.amount)}</td></tr>)}</tbody></table></div>
        </section>
    </main>;
}
