# AGENTS.md — DingFood.API

Regras específicas da camada de apresentação. Complementa o `backend/AGENTS.md` — leia aquele
primeiro para arquitetura, `Result`/`Error`, multi-tenancy e a receita de feature. Aqui só o que
é próprio da API.

---

## 1. O que esta camada é

Host ASP.NET Core. Traduz HTTP em `IRequest` do MediatR e `Result` em status code. **Não contém
regra de negócio** — controller que faz `if` de domínio está no lugar errado.

É a única camada que ninguém referencia: nem Application nem Infrastructure podem depender dela
(`Infrastructure_ShouldNotDependOn_Api`).

```
Controllers/       ApiController (base) + ~60 controllers
Middleware/        ExceptionHandling, CompanyContext, WorkplaceContext
Authorization/     FeatureAccessConvention, FeatureAuthorizationHandler, FeatureRequirement
Serialization/     UtcDateTimeConverter
Services/          CurrentUserService
Properties/        launchSettings.json
wwwroot/           /uploads/products — imagens do cardápio
Program.cs
appsettings*.json
```

---

## 2. A ordem do pipeline é crítica

```csharp
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
UseDocsOrTransportSecurity(app);
app.UseStaticFiles();
app.UseCors("Default");
app.UseRateLimiter();
app.UseAuthentication();
app.UseMiddleware<CompanyContextMiddleware>();     // ← reescreve o ClaimsPrincipal
app.UseMiddleware<WorkplaceContextMiddleware>();   // ← reescreve o ClaimsPrincipal
app.UseAuthorization();
app.MapControllers();
```

**Não reordene sem entender o parágrafo abaixo.** Os dois middlewares de contexto precisam rodar
*depois* de `UseAuthentication` (precisam do usuário autenticado) e *antes* de `UseAuthorization`
(a autorização precisa das roles que eles produzem).

---

## 3. As claims do token não são confiáveis — são reescritas

Este é o ponto mais importante da camada, e não é óbvio lendo um controller.

`CompanyContextMiddleware` e `WorkplaceContextMiddleware` **descartam e reconstroem** as claims de
tenant, papel e permissão a cada requisição:

```csharp
foreach (var claim in identity.Claims.Where(x =>
    x.Type is "companyId" or "businessGroupId" or "employeeId" or "permission"
    || x.Type == ClaimTypes.Role).ToArray())
    identity.RemoveClaim(claim);

// ... e re-adiciona a partir do que o BANCO autoriza
identity.AddClaim(new Claim("companyId", company.CompanyId.ToString()));
identity.AddClaims(company.Roles.Select(x => new Claim(ClaimTypes.Role, x)));
identity.AddClaims(company.Permissions.Select(x => new Claim("permission", x)));
context.User = new ClaimsPrincipal(identity);
```

O fluxo real:

1. Cliente manda `Authorization: Bearer <jwt>` + `X-Company-Id` + `X-Branch-Id`.
2. `CompanyContextMiddleware` pega o `X-Company-Id` (ou a claim, como fallback) e pergunta ao
   `ICompanyAccessService` se **aquele usuário** tem acesso **àquela empresa**. Não tendo → **403**.
3. Mesma coisa em `WorkplaceContextMiddleware` para `X-Branch-Id` via `IWorkplaceAccessService`.
4. Roles e permissões passam a ser as que o banco devolveu **para aquela empresa/filial**.

Consequências que você precisa ter em mente:

- **`[Authorize(Roles = "Administrador")]` avalia a role resolvida no servidor**, não a que veio
  no token. Um usuário pode ser administrador na empresa A e não ser na B, com o mesmo token.
- Token de `Customer` não pode trocar de empresa: o middleware exige que o `X-Company-Id` bata
  exatamente com a claim, justamente para impedir que um cliente se passe por `AppUser` com o
  mesmo Id numérico.
- Três rotas pulam o `WorkplaceContextMiddleware` porque precisam rodar **antes** de existir
  contexto operacional: `/api/companies/allowed`, `/api/companies/switch`, `/api/workplaces`.
  Endpoint novo de bootstrap de organização precisa entrar nessa lista.
- Endpoint `[AllowAnonymous]` (ou sem `[Authorize]`) passa direto pelos dois — sem tenant. É o
  caso do cadastro, storefront e das consultas de CNPJ/CEP.

---

## 4. Controllers

Herdam `ApiController`, que já traz `[ApiController]`, `[Route("api/[controller]")]`, o `IMediator`
e o `HandleFailure`.

```csharp
public sealed class SuppliersController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    [HttpGet("company/{companyId:long}")]
    public Task<IActionResult> GetByCompany(long companyId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(SuppliersController), nameof(GetByCompany), async () =>
        {
            var result = await Mediator.Send(new GetSuppliersByCompanyQuery(companyId), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });
}
```

Regras:

- `sealed`, construtor primário, `: ApiController(mediator)`.
- **Não repita `[Route]`** — a base já define `api/[controller]`.
- Toda action envolvida em `ExecuteWithLogAsync` (grava `LogTracker` com classe, método, sucesso,
  duração, erro e IP).
- Corpo da action: `Mediator.Send(...)` e o ternário `IsFailure ? HandleFailure(result) : Ok(...)`.
  Nada além disso.
- Constraint de rota tipada: `{id:long}`, não `{id}`.
- `CancellationToken ct` em toda action, repassado ao `Send`.
- `ManagerRoles` é const da base (`"Administrador"`).

Há duas sobrecargas de `ExecuteWithLogAsync`: a de 5 argumentos (repositórios explícitos, usada
pela maioria) e a de 3 (resolve do `HttpContext.RequestServices`). Prefira a de 5 — é a
predominante e deixa a dependência visível.

### Nomeação

O padrão dominante é **plural para recurso** (`SuppliersController`, `ProductsController`,
`OrdersController`) e **singular para área ou serviço** (`CnpjController`, `CepController`,
`FinanceController`, `PrintingController`). A base já usa o nome sem o sufixo como rota, então
`SuppliersController` → `api/suppliers`.

⚠️ **Renomear um controller pode desligar a proteção dele.** O `FeatureAccessConvention` casa pelo
`ControllerName`; se o nome sair do dicionário, a checagem de feature some silenciosamente — sem
erro de compilação e sem teste falhando. Ver §5.

---

## 5. Autorização por feature

Três peças:

**`FeatureAccessConvention`** — dicionário estático que amarra controller → features, aplicado
como `AuthorizeFilter` com policy `Feature:<lista>`:

```csharp
["Orders"] = "Salao,Preparo,Caixa",
["Products"] = "Cardapio,Salao,Estoque",
["Stock"] = "Estoque",
```

**`FeatureRequirement`** + **`FeatureAuthorizationHandler`** — avaliam a policy:

```csharp
if (FeatureCodes.ManagerRoles.Any(context.User.IsInRole)) { context.Succeed(requirement); return; }

var result = await mediator.Send(new GetMyFeaturesQuery(userId, false));
if (result.IsSuccess && requirement.FeatureCode.Split(',').Any(result.Value.Features.Contains))
    context.Succeed(requirement);
```

Pontos de atenção:

- A lista é **OU**, não E — basta o usuário ter uma das features.
- `Administrador` e `Gerente` passam sempre (short-circuit antes da consulta).
- Para os demais, **cada requisição a controller protegido dispara uma query no banco**. É custo
  conhecido; não multiplique adicionando checagem de feature em lugares extras.
- Esconder o menu no frontend não é proteção — o comentário no handler diz isso explicitamente.
  Controller novo com tela associada **precisa** entrar no dicionário.
- Os códigos vêm de `FeatureCodes.cs` no Domain, e precisam existir no seed.

---

## 6. Erros: dois caminhos, não confunda

**Caminho 1 — `Result` de negócio.** O handler devolve `Result.Failure`, o controller chama
`HandleFailure`, que mapeia **pelo sufixo do `Error.Code`**: `.Forbidden` → 403, `.NotFound` → 404,
`.AlreadyExists`/`.Duplicate` → 409, resto → 400. Detalhe em `backend/AGENTS.md` §4.

**Caminho 2 — exceção.** `ExceptionHandlingMiddleware` captura:

| Exceção | Status | Title |
|---|---|---|
| `TenantAccessException` | 403 | `Tenant.Forbidden` |
| `DeliveryPricingException` | 422 | `Delivery.Pricing` |
| qualquer outra | 500 | `Erro interno` |

Em 500, o `Detail` só traz o stack em `Development`; em produção vira mensagem genérica. **Não
mexa nisso** — é o que impede vazamento de detalhe interno.

O `ApiController.ExecuteWithLogAsync` também captura `TenantAccessException` e devolve 403, antes
de o middleware ver. Duplicação inofensiva; os dois existem porque nem todo caminho passa pelo
controller.

Toda resposta de erro é `ProblemDetails` com `Title` = `Error.Code` e `Detail` = mensagem — é esse
contrato que o `apiClient.ts` do frontend consome como `ApiError.code` / `ApiError.message`.

---

## 7. Serialização de datas — há uma contradição aqui

`UtcDateTimeConverter` está registrado globalmente:

```csharp
builder.Services.AddControllers(...).AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter()));
```

Ele assume que **tudo que vem do banco está em UTC** e, quando o `Kind` é `Unspecified` (que é o
que o EF Core devolve), carimba `Z` na saída:

```csharp
_ => DateTime.SpecifyKind(value, DateTimeKind.Utc), // Unspecified: o banco guarda UTC
```

⚠️ **Mas as entidades gravam `DateTime.Now`, que é hora local, não UTC** (ver
`DingFood.Domain/AGENTS.md` §5). As duas premissas só coincidem quando o processo roda com fuso
UTC — o que é verdade em container Linux com TZ padrão, e **falso** em máquina de desenvolvimento
no fuso de São Paulo.

Efeito prático: rodando localmente, horários salvos podem sair da API 3 horas adiantados, porque
um instante local é rotulado como se fosse UTC. Se você for mexer em data, saiba que existe essa
divergência entre camadas — a correção de verdade é escolher um dos dois lados (gravar `UtcNow`
nas entidades **ou** ajustar o converter) e migrar o que já está gravado.

Fora isso, JSON é camelCase (padrão do ASP.NET, sem `PropertyNamingPolicy` customizada).

---

## 8. Program.cs

Organizado em funções estáticas locais: `ConfigureLogging`, `ValidateDatabaseConnectionString`,
`ConfigureAuthentication`, `ConfigureCors`, `ConfigureRateLimiting`, `ConfigureSwagger`,
`ValidateJwtSecret`, `SeedDefaultCashRegisterAsync`, `UseDocsOrTransportSecurity`.

Ao adicionar configuração, **crie ou estenda uma função nomeada** em vez de engordar o corpo.

### Rate limiting (já existe — use)

```csharp
GlobalLimiter: 200 req/min por IP (janela fixa)
Policy "auth":  10 req/min por IP
RejectionStatusCode: 429
```

Endpoint anônimo sensível — login, cadastro, consulta que faz proxy para serviço de terceiro —
deve levar `[EnableRateLimiting("auth")]`.

### Registro de serviço

A API registra praticamente nada: `AddApplication()` e `AddInfrastructure(configuration)` fazem o
trabalho. A exceção é `ICurrentUserService` → `CurrentUserService`, registrado direto no
`Program.cs` porque depende de `IHttpContextAccessor` e é próprio da apresentação.

`CurrentUserService.UserId` **lança** `InvalidOperationException` se não houver `HttpContext` ou
claim. Não use em background service — lá o caminho é `CurrentTenantService.SetBackgroundCompany`
(ver `DingFood.Infrastructure/AGENTS.md` §6).

### Validação na subida

`ValidateDatabaseConnectionString` e `ValidateJwtSecret` derrubam a aplicação se a configuração
estiver ausente ou com valor de exemplo. É proposital — falhar na subida é melhor que subir
inseguro.

### Seed

`SeedDefaultCashRegisterAsync` roda a cada subida, é idempotente e nunca impede a API de subir
(try/catch com log). **Não** aplica migrations — ver `backend/AGENTS.md` §9.

### Health check

`GET /health` com `AddMySql`, tag `ready`, e `ResponseWriter` customizado devolvendo versão via
`APP_VERSION`.

---

## 9. Ambiente e segredos

`launchSettings.json`:

```
https://localhost:7250 ; http://localhost:5250
```

⚠️ O proxy do Vite aponta para **`http://localhost:5250`**. Rodando a API no perfil HTTPS, o
frontend leva 502. Ver `frontend/AGENTS.md` §9.

⚠️ **`launchSettings.json` e `appsettings.json` estão versionados com a connection string de
produção, usuário e senha em claro** — e o `appsettings.json` tem `Jwt.Secret` de exemplo. Não
adicione credencial nova nesses arquivos; use variável de ambiente ou secret do ambiente de
deploy. As que já estão ali deveriam ser rotacionadas e removidas do histórico.

---

## 10. Adicionar um endpoint

1. Command/Query + Handler na Application (`DingFood.Application/AGENTS.md`).
2. Controller herdando `ApiController`, action envolvida em `ExecuteWithLogAsync`.
3. Escolher o `Error.Code` no handler pensando no status que você quer (§6).
4. Autorização:
   - precisa de login? → `[Authorize]` (ou nada, se a policy do controller já cobre)
   - só gestor? → `[Authorize(Roles = ManagerRoles)]`
   - pertence a uma tela? → entrada em `FeatureAccessConvention`
   - público? → sem `[Authorize]`, e avalie `[EnableRateLimiting("auth")]`
5. Se o request carrega escopo, nomear a propriedade `CompanyId` / `...BranchId` para o
   `TenantRequestBehavior` validar (`backend/AGENTS.md` §6).

---

## 11. Armadilhas conhecidas

1. **Renomear controller desliga a feature gate** silenciosamente. §4.
2. **Reordenar o pipeline** quebra autorização — os middlewares de contexto produzem as roles. §2.
3. **`HandleFailure` não mapeia 429** nem 422 vindo de `Result`; sufixo desconhecido vira 400. §6.
4. **Converter de data assume UTC, entidades gravam local.** §7.
5. **Endpoint de bootstrap novo** precisa entrar na lista de exceções do
   `WorkplaceContextMiddleware`, senão fica em deadlock de contexto. §3.
6. **Credencial versionada** em `appsettings.json` e `launchSettings.json`. §9.
7. **`FeatureAuthorizationHandler` consulta o banco por requisição** para não-gestores. §5.
8. `Program.cs` **não** aplica migrations.

---

## 12. Checklist antes do PR

- [ ] Controller `sealed`, herdando `ApiController`, sem `[Route]` repetido
- [ ] Action envolvida em `ExecuteWithLogAsync`, com `CancellationToken` repassado
- [ ] Corpo da action só faz `Send` + `HandleFailure`/`Ok` — zero regra de negócio
- [ ] Constraint de rota tipada (`{id:long}`)
- [ ] Autorização decidida explicitamente (`[Authorize]`, roles, feature ou público consciente)
- [ ] Controller de tela registrado em `FeatureAccessConvention`
- [ ] Endpoint anônimo sensível com `[EnableRateLimiting("auth")]`
- [ ] Nenhuma credencial nova em `appsettings.json` / `launchSettings.json`
- [ ] Configuração nova em função nomeada no `Program.cs`
- [ ] `dotnet test test/DingFood.ArchTests` verde
