# AGENTS.md — DingFood.Infrastructure

Regras específicas da camada de infraestrutura. Complementa o `backend/AGENTS.md` — leia aquele
primeiro para arquitetura geral, convenções de banco, migrations e CI. Aqui só o que é próprio
da Infrastructure.

---

## 1. O que esta camada é

A única camada que fala com o mundo: banco, HTTP, disco, impressora, relógio, criptografia.
Ela **implementa** as interfaces declaradas em `DingFood.Application/Abstractions/` e em
`DingFood.Domain/Repositories/`.

Regra de dependência: pode referenciar Domain e Application, **não** pode referenciar API
(`Infrastructure_ShouldNotDependOn_Api`). Tem `FrameworkReference Microsoft.AspNetCore.App`, então
`IHttpContextAccessor`, Data Protection e `ILogger` estão disponíveis aqui — e só aqui.

```xml
<AllowUnsafeBlocks>true</AllowUnsafeBlocks>   <!-- exigido pelo LibraryImport em WindowsRawPrinterTransport -->
<InternalsVisibleTo Include="DingFood.Tests" />
```

⚠️ `AllowUnsafeBlocks` é switch do projeto inteiro. Existe por causa do P/Invoke da impressora
Windows; não é convite para usar `unsafe` em outro lugar.

```
Authentication/   JwtTokenProvider, PasswordHasher, ReadingProofService, JwtOptions
Delivery/         GoogleDeliveryGeocoder
Fiscal/           FakeFiscalDocumentService
Integrations/     Asaas, IFood, Keeta, WhatsApp, Cnpja, ViaCep
Payments/         FakePaymentGatewayService
Persistence/      AppDbContext (4 partials), Configurations, Repositories, Migrations
Printing/         PrintingService, TicketFormatter, EscPos, transports
Security/         DataProtectionSecretProtector
Storage/          LocalImageStorage
Tenancy/          CurrentTenantService, PublicWorkplaceScope, WorkplaceAccessService, CompanyAccessService
Time/             TimeProviderCustom
DependencyInjection.cs
```

Implementações são **`internal sealed`**. Só o que a API precisa instanciar por nome fica público
(os clients de integração, por exemplo). Repositórios e Configurations têm teste de arquitetura
exigindo `internal sealed` e o namespace certo.

---

## 2. `DependencyInjection.cs` — ponto único de registro

**Todo serviço da Infrastructure é registrado em `AddInfrastructure`.** Não existe registro
espalhado por arquivo nem varredura por convenção (exceto as EF Configurations, descobertas por
`ApplyConfigurationsFromAssembly`). Serviço novo sem linha aqui simplesmente não existe em runtime
— foi exatamente assim que a integração do CNPJ quebrou na primeira subida.

### Tempos de vida em uso

| Lifetime | O quê |
|---|---|
| `Scoped` | `AppDbContext`, todos os repositórios, `IUnitOfWork`, resolvers de credencial, `ICurrentTenantService`, `IPrintingService` |
| `Singleton` | `TimeProvider`, `IImageStorage`, `IPasswordHasher`, `IJwtTokenProvider`, `ISecretProtector`, `IReadingProofService`, stores em memória |
| `AddHttpClient<TInterface, TImpl>` | todo client de integração |
| `AddHostedService` | background services |

`IUnitOfWork` resolve a **mesma instância** do `AppDbContext` do escopo:

```csharp
services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
```

### Um objeto servindo vários papéis

Quando a mesma instância precisa atender mais de uma interface, registre a **classe concreta** e
faça as interfaces apontarem para ela — nunca registre a implementação duas vezes, ou você ganha
dois objetos e um estado dividido.

```csharp
services.AddSingleton<WhatsAppOutbox>();
services.AddSingleton<IWhatsAppQueue>(sp => sp.GetRequiredService<WhatsAppOutbox>());
services.AddHostedService(sp => sp.GetRequiredService<WhatsAppOutbox>());   // fila E worker

services.AddScoped<AsaasService>();
services.AddScoped<IAsaasService>(sp => sp.GetRequiredService<AsaasService>());
```

### Data Protection

```csharp
var keysFolder = Path.Combine(AppContext.BaseDirectory, "app_data", "protecting-keys");
services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("DingFood");
```

⚠️ **Essa pasta precisa sobreviver a deploy e a recriação de container.** Se as chaves se
perderem, todo segredo já cifrado no banco (`ClientSecretEncrypted` das integrações) vira lixo
ilegível — e não há como recuperar, só reconfigurar cada integração à mão. Em Docker, monte um
volume.

---

## 3. Persistência

Convenções de tabela, coluna, índice e migration estão em `backend/AGENTS.md` §8 e §9. Aqui, o que
é específico do `AppDbContext`.

### Quatro partials, papéis distintos

| Arquivo | Papel |
|---|---|
| `AppDbContext.cs` | `DbSet<>`, `OnModelCreating`, `CommitAsync` |
| `AppDbContext.Tenancy.cs` | query filters por empresa/filial, escritos à mão |
| `AppDbContext.OperationalScopes.cs` | classificação automática de escopo + **a trava** |
| `AppDbContext.Delivery.cs` | regras de entrega |

`OnModelCreating` chama `ApplyConfigurationsFromAssembly` — Configuration nova é descoberta
sozinha. O **`DbSet` não é**: precisa ser declarado à mão em `AppDbContext.cs`.

### A trava de escopo

`ConfigureOperationalScopes` classifica toda entidade do modelo e lança se sobrar alguma:

```
InvalidOperationException: Entities without ownership: <Entidade>
```

Isso quebra `dotnet ef migrations add` **e** a subida da API. As categorias e como declarar uma
tabela global (`SystemTables`) estão em `backend/AGENTS.md` §6. É a armadilha número um desta
camada.

### CommitAsync

```csharp
public async Task<int> CommitAsync(CancellationToken ct = default)
{
    try { return await SaveChangesAsync(ct); }
    catch (DbUpdateConcurrencyException ex) { throw new ConcurrencyException("…", ex); }
    catch (DbUpdateException ex) { throw new ConcurrencyException("…", ex); }
}
```

Não há despacho de domain events aqui — ver `DingFood.Domain/AGENTS.md` §7.

### Repositórios

`internal sealed`, construtor primário recebendo `AppDbContext`. Leitura com `AsNoTracking()`,
escrita sem. Nunca chamam `SaveChanges`. Detalhe em `backend/AGENTS.md` §8.

`IgnoreQueryFilters()` **só** em consulta de background service, que roda sem tenant HTTP:

```csharp
public async Task<IReadOnlyCollection<IfoodCompanyScope>> GetEnabledScopesAsync(CancellationToken ct = default)
    => await context.IfoodIntegrationSettings.IgnoreQueryFilters().AsNoTracking()
        .Where(s => s.IsActive && s.Enabled)
        .Select(s => new IfoodCompanyScope(s.CompanyId, s.BrandId)).Distinct().ToListAsync(ct);
```

Usar isso em caminho de requisição HTTP é furo de isolamento entre empresas. Se precisou, revise.

---

## 4. Clients de integração

Estrutura por provedor em `Integrations/<Provedor>/`:

| Arquivo | Papel |
|---|---|
| `<Provedor>Settings.cs` | POCO com `const string SectionName`, ligado por `Configure<T>` |
| `<Provedor>OptionsProvider.cs` | adapta `IOptions<Settings>` para a interface que a Application lê |
| `<Provedor>CredentialsResolver.cs` | resolve credencial **por tenant** (§5) |
| `<Provedor>Client.cs` | `HttpClient` tipado |
| `<Provedor>...BackgroundService.cs` | worker (§6) |

O client **traduz falha remota em `Result.Failure`** com `Error.Code` no formato
`<Provedor>.<Motivo>`. Só deixa escapar exceção realmente inesperada. Referência: `CnpjaClient`
e `ViaCepClient`.

Padrão de construtor:

```csharp
public CnpjaClient(HttpClient httpClient, IOptions<CnpjaSettings> options, ILogger<CnpjaClient> logger)
{
    _http = httpClient;
    var settings = options.Value;

    if (_http.BaseAddress is null && !string.IsNullOrWhiteSpace(settings.BaseUrl))
        _http.BaseAddress = new Uri(settings.BaseUrl.TrimEnd('/') + "/");

    if (_http.DefaultRequestHeaders.Accept.Count == 0) { ... }   // guarda contra reconfiguração
}
```

As guardas `if (... is null)` / `if (Count == 0)` existem porque o `HttpClient` tipado pode ser
reaproveitado — não assuma instância limpa.

Consulta a serviço externo com limite de uso **precisa de cache no banco**: entidade snapshot com
`RawJson` (`longtext`), `QueriedAt` e TTL configurável, e fallback para o dado vencido quando o
serviço cai. `CnpjQuery` e `CepQuery` são os modelos.

---

## 5. Credencial por tenant

Integração cuja credencial varia por empresa/filial usa um **resolver**, nunca lê `appsettings`
direto no client:

```csharp
internal sealed class KeetaCredentialsResolver(
    IKeetaIntegrationSettingRepository settingRepository,
    IOptions<KeetaSettings> options) : IKeetaCredentialsResolver
{
    public async Task<KeetaCredentials> ResolveAsync(long companyId, long branchId, CancellationToken ct = default)
    {
        var setting = await settingRepository.GetByBranchOrCompanyFallbackAsync(companyId, branchId, ct);

        if (setting is not null && !string.IsNullOrWhiteSpace(setting.ClientId) && ...)
            return new KeetaCredentials(setting.BaseUrl ?? _settings.BaseUrl, setting.ClientId, ...);

        return new KeetaCredentials(_settings.BaseUrl, _settings.ClientId, ...);   // fallback global
    }
}
```

Cadeia: **configuração da filial → configuração da empresa → `appsettings`**. O resolver é
`Scoped` e o client o chama antes de cada operação.

### Segredo persistido

Nunca grave `ClientSecret` em claro. Use `ISecretProtector` com um `purpose` fixo e versionado:

```csharp
var encrypted = secretProtector.Protect("DingFood.Integrations.Ifood.ClientSecret.v1", clientSecret);
```

⚠️ Trocar a string de `purpose` torna ilegível tudo que já foi cifrado com a anterior. Se
precisar rotacionar, incremente a versão **e** escreva a migração dos registros existentes.

---

## 6. Background services — leia antes de criar um

É onde mais se erra, porque o modelo de escopo é diferente do resto da aplicação.

```csharp
internal sealed class IfoodOrderPollingBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<IfoodOrderPollingBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // atraso inicial: deixa a API terminar de subir
        try { await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); }
        catch (OperationCanceledException) { return; }

        using var timer = new PeriodicTimer(PollInterval);
        while (!stoppingToken.IsCancellationRequested)
        {
            try { await RunCycleAsync(stoppingToken); }
            catch (Exception ex) { logger.LogError(ex, "Ciclo falhou inesperadamente."); }

            try { if (!await timer.WaitForNextTickAsync(stoppingToken)) break; }
            catch (OperationCanceledException) { break; }
        }
    }
}
```

Regras não negociáveis:

**1. `BackgroundService` é singleton.** Não injete `DbContext`, repositório ou `IMediator` no
construtor — todos são `Scoped`. Crie um escopo por ciclo:

```csharp
using var scope = serviceProvider.CreateScope();
var repo = scope.ServiceProvider.GetRequiredService<IIfoodIntegrationSettingRepository>();
```

**2. Estabeleça o tenant explicitamente.** Fora de HTTP não há claim `companyId`, então os query
filters ficariam desligados e o job veria dados de todas as empresas. O mecanismo é um escopo por
empresa + `SetBackgroundCompany`:

```csharp
using var companyScope = serviceProvider.CreateScope();
companyScope.ServiceProvider.GetRequiredService<CurrentTenantService>()
    .SetBackgroundCompany(companyId, brandId);

var mediator = companyScope.ServiceProvider.GetRequiredService<IMediator>();
await mediator.Send(new SyncIfoodOrdersCommand(companyId), ct);
```

`SetBackgroundCompany` lança `InvalidOperationException` se houver `HttpContext` ou se for
chamado duas vezes com empresas diferentes — *"A background scope belongs to exactly one
company."* **Um escopo, uma empresa.** Nunca reaproveite escopo entre empresas do laço.

**3. Uma exceção não pode matar o loop.** `try/catch` em volta do ciclo **e** dentro do laço por
empresa, sempre com `logger.LogError`.

**4. Paralelismo limitado:**

```csharp
await Parallel.ForEachAsync(companyIds.Distinct(),
    new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = ct },
    async (scope, token) => { ... });
```

Sem limite, N empresas abrem N conexões MySQL simultâneas.

**5. Trabalho de verdade vai para um Command do MediatR**, não para dentro do worker. O worker
agenda; a lógica fica na Application, testável.

**6. `PeriodicTimer`**, não `Task.Delay` no fim do laço — o timer não acumula atraso do
processamento.

Registro: `services.AddHostedService<XBackgroundService>();`

---

## 7. Tenancy — as três formas de resolver a empresa

`CurrentTenantService` é `Scoped` e resolve `CompanyId` por três caminhos, nesta ordem:

1. **`_resolvedWorkplace`** — definido pelo `WorkplaceContextMiddleware` (na API) via
   `SetResolvedWorkplace(companyId, brandId, branchId)`. Chamar de novo com valores diferentes
   lança `TenantAccessException`.
2. **Claim `companyId` do JWT** — o caminho normal de requisição autenticada.
3. **`_backgroundCompanyId`** — definido por `SetBackgroundCompany` (§6).

Se os três forem nulos, `CompanyId` é `null` e **os query filters não se aplicam**. Isso é
proposital (login, refresh, seed, migration), mas significa que código rodando sem tenant vê tudo.
Ao escrever serviço novo que roda fora de requisição, decida explicitamente qual empresa ele
atende.

`PublicWorkplaceScope` cobre o fluxo pré-login (storefront, checkout, pedido por token) — é
amarrado pelo `TenantRequestBehavior`, não manualmente.

---

## 8. Implementações provisórias

Duas interfaces têm implementação **fake** registrada hoje:

```csharp
services.AddScoped<IPaymentGatewayService, FakePaymentGatewayService>();
services.AddScoped<IFiscalDocumentService, FakeFiscalDocumentService>();
```

`FakePaymentGatewayService` aprova qualquer cobrança instantaneamente e devolve um payload Pix
inventado. O próprio arquivo diz: *"Substitua por uma implementação real antes de ir para
produção."*

Isso **não** significa que o projeto não cobra — o caminho real de pagamento é o Asaas
(`IAsaasService`, `Features/Checkout/PayOrderWith*`). O `IPaymentGatewayService` é uma abstração
paralela ainda não conectada. Saiba qual das duas você está tocando antes de mexer.

---

## 9. Detalhes que economizam tempo

**Tempo.** `TimeProviderCustom` sobrescreve só `GetUtcNow()`; `GetLocalNow()` vem da classe base,
convertendo pelo fuso da máquina. O código chama `GetLocalNow().DateTime` porque o projeto
trabalha em hora local (ver `DingFood.Domain/AGENTS.md` §5). Registrado como `Singleton`.

**Logging.** `ILogger<T>` por injeção, com template estruturado e placeholder nomeado:

```csharp
logger.LogWarning("CNPJá retornou HTTP {Status} para {TaxId}: {Body}", (int)response.StatusCode, taxId, body);
```

Nunca interpole string no template — perde a estrutura no Serilog. E nunca logue segredo,
token ou corpo de requisição com dado pessoal.

**Impressão.** `PrintingService` é `Scoped`; os transports são `Singleton` e há **dois
registrados para a mesma interface** (`WindowsRawPrinterTransport` e `NetworkRawPrinterTransport`)
— o serviço escolhe em tempo de execução. Ao injetar, receba `IEnumerable<IRawPrinterTransport>`,
não a interface direta, ou você pega só o último registrado.

**Scripts `.sql` embarcados.** `Persistence/Migrations/*.sql` (`IfoodWebhook.sql`,
`BusinessGroups.sql`) estão como `EmbeddedResource` no `.csproj`, mas **nada no código os
executa** — são legado anterior às migrations, aplicados à mão. Não crie novos; use EF migrations
(`backend/AGENTS.md` §9).

**Nome de arquivo x classe.** Existem arquivos `IFoodXxx.cs` com classes `IfoodXxx`. Ao criar
arquivo novo faça bater; ao mexer em existente, siga o nome da classe.

---

## 10. Testar

`InternalsVisibleTo` libera `DingFood.Tests`, então implementação `internal` é testável
diretamente. O projeto de testes tem `Microsoft.EntityFrameworkCore.Sqlite` para exercitar
repositório e `AppDbContext` em memória.

- **Repositório**: `AppDbContext` sobre SQLite in-memory, verificando filtro e projeção.
- **Client de integração**: `HttpMessageHandler` falso devolvendo resposta canned; teste cada
  ramo de erro (404, 429, timeout, corpo inválido) e confirme o `Error.Code` correspondente.
- **Background service**: extraia a lógica para o Command do MediatR e teste o Command. O worker
  em si é só agendamento.

Testes em `test/DingFood.Tests/Infrastructure/`.

---

## 11. Checklist antes do PR

- [ ] Implementação `internal sealed` (obrigatório para repositório e Configuration)
- [ ] **Registrado em `DependencyInjection.cs`** com o lifetime certo
- [ ] Entidade nova: `DbSet` declarado **e** escopo classificado (`backend/AGENTS.md` §6)
- [ ] Repositório não chama `SaveChanges`; leitura com `AsNoTracking()`
- [ ] `IgnoreQueryFilters()` só em background service, nunca em caminho HTTP
- [ ] Client de integração devolve `Result.Failure` com `Error.Code` `<Provedor>.<Motivo>`
- [ ] Credencial por tenant via resolver, com fallback para `appsettings`
- [ ] Segredo gravado via `ISecretProtector` com `purpose` versionado
- [ ] Background service: escopo por ciclo, escopo por empresa, `SetBackgroundCompany`,
      try/catch por empresa, paralelismo limitado, `PeriodicTimer`
- [ ] Log estruturado, sem segredo e sem dado pessoal
- [ ] Nenhum `using` de `DingFood.API`
- [ ] `dotnet test test/DingFood.ArchTests` verde
