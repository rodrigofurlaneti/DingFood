# AGENTS.md — DingFood.Application

Regras específicas da camada de aplicação. Complementa o `backend/AGENTS.md` — leia aquele
primeiro para arquitetura, multi-tenancy, persistência, migrations e CI. Aqui só o que é próprio
da Application.

---

## 1. O que esta camada é

Orquestração de casos de uso. Carrega agregado, chama método de domínio, persiste, devolve DTO.
**Regra de negócio mora no Domain**, não aqui — se um handler está decidindo cálculo ou transição
de estado, provavelmente há um método faltando na entidade.

Dependências permitidas (o `.csproj` é a fronteira real):

```
MediatR 12 · FluentValidation 11 · Microsoft.Extensions.DependencyInjection.Abstractions
Microsoft.Extensions.Caching.Memory · BCrypt.Net-Next
→ ProjectReference: DingFood.Domain (só)
```

O teste `Application_ShouldNotDependOn_InfrastructureOrApi` proíbe Infrastructure, API e
**EF Core**. Não há `Microsoft.AspNetCore.*` aqui — é class library pura. Precisou de framework
web, Data Protection, HttpClient ou banco? Vira interface em `Abstractions/` e implementação na
Infrastructure.

`InternalsVisibleTo` libera `DingFood.Tests` e `DingFood.Specs` — por isso handler pode ser
`internal sealed` e ainda assim ser testado.

---

## 2. Mapa da camada

```
Abstractions/
  Messaging/        ICommand, IQuery, BaseCommandHandler, BaseQueryHandler
  Tenancy/          ICurrentTenantService, TenantRequestBehavior, ICompanyAccessService...
  Integrations/<Provedor>/   contrato do parceiro: I<X>Client + DTOs + I<X>Options
  Authentication/ Security/ Storage/ Printing/ Payments/ Fiscal/ Notifications/
Features/<Área>/
  <Ação>/           Command|Query + Handler + Validator
  <Área>Response.cs contratos de saída
  <Área>Mapping.cs  entidade -> Response
  Shared/           serviço de aplicação compartilhado pela área
Common/             quase vazio — ver §9
DependencyInjection.cs
```

`Abstractions/` é o lugar de contrato com o mundo externo. `Features/` é o lugar de caso de uso.
Nada de lógica de negócio solta na raiz.

---

## 3. Granularidade: uma pasta por caso de uso

`Features/Orders/` tem 25 subpastas: `AddItem`, `AddPizzaItem`, `ApplyDiscount`, `Cancel`,
`Close`, `Open`, `Reopen`, `SplitBill`, `TransferItem`, `UpdateItemStatus`…

A pasta leva o nome do **verbo do caso de uso**, não do CRUD. Não agrupe cinco ações num handler
com `switch`. Se a ação tem nome próprio na boca do usuário ("fechar conta", "transferir item"),
ela tem pasta própria.

Dentro, três arquivos no máximo:

```
Close/CloseOrderCommand.cs
Close/CloseOrderCommandHandler.cs
Close/CloseOrderCommandValidator.cs
```

---

## 4. Command vs Query

```csharp
public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
```

**Escreveu no banco → Command.** Só leu → Query. O critério é a escrita, não o verbo HTTP: um
`GET /api/cnpj/{taxId}` que grava snapshot em cache é servido por `ConsultCnpjCommand`, porque
precisa do `IUnitOfWork` que só o `BaseCommandHandler` carrega.

Records posicionais, sempre `sealed`:

```csharp
public sealed record ConsultCepCommand(string Cep, bool ForceRefresh = false) : ICommand<CepResponse>;
public sealed record GetSuppliersByCompanyQuery(long CompanyId) : IQuery<IReadOnlyCollection<SupplierResponse>>;
```

⚠️ **Nome de propriedade importa.** `CompanyId` e qualquer coisa terminada em `BranchId` são
inspecionadas por reflexão pelo `TenantRequestBehavior` e validadas contra o tenant da sessão
(§7). Nomeie assim quando o campo for de fato o escopo — e **não** use esses nomes para outra
coisa.

---

## 5. Handlers

`internal sealed`, herdando a base correspondente (há teste de arquitetura para ambos).

```csharp
internal sealed class CreateSupplierCommandHandler(
    ISupplierRepository supplierRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<CreateSupplierCommand, long>(logRepository, unitOfWork)
{
    public override async Task<Result<long>> Handle(CreateSupplierCommand request, CancellationToken ct)
    {
        return await ExecuteWithLogAsync(
            nameof(CreateSupplierCommandHandler), nameof(Handle), null,
            async (userIdBox) =>
            {
                var supplier = Supplier.Create(request.CompanyId, request.LegalName, ...);
                if (supplier.IsFailure) return Result.Failure<long>(supplier.Error);

                await supplierRepository.AddAsync(supplier.Value, ct);
                await unitOfWork.CommitAsync(ct);

                return Result.Success(supplier.Value.Id);
            });
    }
}
```

Quatro bases disponíveis:

| Base | Para |
|---|---|
| `BaseCommandHandler<TRequest, TResponse>` | comando que devolve valor |
| `BaseCommandHandler<TRequest>` | comando sem retorno |
| `BaseQueryHandler<TRequest, TResponse>` | consulta |

Todas exigem `ILogTrackerRepository` + `IUnitOfWork` no construtor e expõem
`ExecuteWithLogAsync`, que grava um `LogTracker` (classe, método, sucesso, duração, erro,
stack, IP) **na mesma unidade de trabalho**.

Regras do `ExecuteWithLogAsync`:

- O `await` na gravação do log é **deliberado**. Já foi `Task.Run` fire-and-forget e causava
  `NullReferenceException` em `ChangeDetector.DetectChanges`, porque a task usava o `DbContext`
  depois do fim do escopo da requisição. O código tem comentário longo sobre isso — não reverta.
- Falha ao gravar log nunca derruba o comando (try/catch interno).
- O `userIdBox` existe para o handler informar quem executou: preencha `userIdBox.Value` quando
  o request trouxer o id do usuário. Muitos handlers passam `null` no IP e não preenchem o box —
  é dívida conhecida, mas preencha no código novo quando a informação existir.

Construtor primário é o padrão no código recente; construtor explícito com campos `_privados`
também existe e é aceito.

---

## 6. Response e Mapping

### Response

`sealed record` (há teste), posicional, em `Features/<Área>/<Área>Response.cs`.

```csharp
public sealed record OrderItemResponse(
    long Id, long ProductId, decimal Quantity, decimal UnitPrice, ...,
    IReadOnlyCollection<OrderItemComplementResponse> Complements)
{
    public IReadOnlyCollection<OrderItemOptionalExtraResponse> OptionalExtras { get; init; } = [];
    public IReadOnlyCollection<OrderItemBoostResponse> Boosts { get; init; } = [];
}
```

**Convenção importante:** campo acrescentado depois entra como `{ get; init; }` com valor padrão,
**não** como novo parâmetro posicional. Mexer na lista posicional quebra toda chamada existente e
todo teste; a propriedade `init` é aditiva. Siga isso ao estender um Response que já existe.

Coleção sempre `IReadOnlyCollection<T>`, nunca `List<T>`.

### Mapping

```csharp
internal static class OrderMapping
{
    internal static OrderResponse ToResponse(this CustomerOrder order, decimal partialPaidAmount = 0)
        => new(order.Id, order.BranchId, ...,
               order.Items.Where(i => i.IsActive).Select(i => new OrderItemResponse(...)).ToList());
}
```

- `internal static class <Área>Mapping`, método de extensão `ToResponse`.
- **Filtre `IsActive` em todos os níveis.** A exclusão é lógica; item desativado não pode vazar
  para o contrato. Esse filtro é fácil de esquecer em coleção aninhada.
- Sem AutoMapper. Mapeamento é escrito à mão, de propósito.
- Não resolva dado de outro agregado só para enriquecer o DTO. `OrderItemComplementResponse`
  manda `ComplementId` sem o nome, e o front casa pelo catálogo que já carregou. Puxar o nome
  aqui custaria uma consulta por item e acoplaria os agregados.

---

## 7. TenantRequestBehavior

Único `IPipelineBehavior` registrado. Roda em **todo** request do MediatR e faz duas coisas:

1. Para features públicas (`PublicOrdering`, `Storefront`, `Checkout`, `Auth.CustomerLogin`, e
   `CustomerAppUser.Create` sem filial), amarra o escopo via `IPublicWorkplaceScope` usando
   `BranchId`, `Token` ou `CustomerOrderId` do request.
2. Havendo tenant autenticado, inspeciona as propriedades do request por reflexão:
   `CompanyId` diferente do da sessão → `TenantAccessException` (403); propriedade terminada em
   `BranchId` → valida que a filial pertence à empresa e está ativa.

**Não escreva checagem manual de tenant no handler.** Nomear a propriedade já garante a barreira.
Se você precisa deliberadamente operar fora do tenant (job, sincronização), o caminho é o
repositório com `IgnoreQueryFilters()`, não burlar o behavior.

A lista de namespaces públicos está **hardcoded** no behavior. Feature pública nova precisa ser
adicionada lá, senão o escopo público não é montado.

---

## 8. Serviços de aplicação (`Features/<Área>/Shared/`)

Quando vários handlers da mesma área repetem uma orquestração, extraia um serviço — **não** um
handler chamando outro handler.

```csharp
// Features/Checkout/Shared/CheckoutOrderPreparer.cs — interface e impl no MESMO arquivo
public interface ICheckoutOrderPreparer
{
    Task<Result<CheckoutOrderPreparation>> PrepareAsync(long customerOrderId, CancellationToken ct = default);
}

public sealed record CheckoutOrderPreparation(CustomerOrder Order, Customer Customer, Branch Branch, string AsaasCustomerId);

internal sealed class CheckoutOrderPreparer(...) : ICheckoutOrderPreparer { ... }
```

Registrado à mão em `Application/DependencyInjection.cs`:

```csharp
services.AddScoped<ICheckoutOrderPreparer, CheckoutOrderPreparer>();
services.AddScoped<IPaymentMethodAvailability, PaymentMethodAvailability>();
services.AddScoped<IKeetaAccessTokenProvider, KeetaAccessTokenProvider>();
services.AddScoped<IKeetaOrderEventProcessor, KeetaOrderEventProcessor>();
```

Convenções: interface `public`, implementação `internal sealed`, ambas no mesmo arquivo, retorno
em `Result<T>`, registro `Scoped`. Os quatro `PayOrderWith*` do Checkout compartilham o
`CheckoutOrderPreparer` — é o exemplo a copiar.

---

## 9. Detalhes que economizam tempo

**Tempo.** Use `TimeProvider` injetado, não `DateTime.Now`:

```csharp
order.Close(0m, timeProviderCustom.GetLocalNow().DateTime);
```

Os métodos de agregado rico recebem o instante por parâmetro justamente para isso
(ver `DingFood.Domain/AGENTS.md` §5). `GetLocalNow()`, não `GetUtcNow()` — o projeto trabalha em
hora local.

**Error.** Duas formas convivem e ambas são aceitas:

```csharp
new Error("Supplier.EmptyLegalName", "…")                       // construtor direto
Error.NotFound("CustomerOrder.NotFound", "Pedido não encontrado.") // fábrica semântica
```

As fábricas (`Validation`, `NotFound`, `Conflict`, `Failure`) só documentam intenção — o que
define o HTTP status é o **sufixo do Code**, não a fábrica. `backend/AGENTS.md` §4.

**Validação.** `AddValidatorsFromAssembly(..., includeInternalTypes: true)` registra os
validators, e o `AddFluentValidationAutoValidation()` da API os executa — mas **só sobre modelo
ligado pelo MVC** (`[FromBody]`, query string). Command montado à mão dentro da controller **não
passa por validador**. Nesses casos, valide dentro do handler e devolva `Result.Failure`.
Escreva o validator mesmo assim. `backend/AGENTS.md` §12.

**`Common/`** tem um único arquivo (`Interfaces/IFood/IIFoodAuthService.cs`) e está praticamente
morto. **Não coloque nada novo lá** — contrato externo vai em `Abstractions/`, orquestração vai em
`Features/<Área>/Shared/`.

**Pastas vazias no `.csproj`.** Existem `<Folder Include="..." />` para
`Abstractions\Integrations\Keeta\` e `Features\Dining\Assignment\Update\`. São resquícios do
Visual Studio, não estrutura a preencher.

---

## 10. Testar

Sem banco e sem HTTP. Repositórios e clients entram como `NSubstitute`:

```csharp
var repo = Substitute.For<ISupplierRepository>();
var log = Substitute.For<ILogTrackerRepository>();
var uow = Substitute.For<IUnitOfWork>();

var handler = new CreateSupplierCommandHandler(repo, log, uow);
var result = await handler.Handle(new CreateSupplierCommand(1, "", null, null, null, null), default);

result.IsFailure.Should().BeTrue();
result.Error.Code.Should().Be("Supplier.EmptyLegalName");
await uow.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
```

Cubra por handler: caminho feliz, cada `Result.Failure` possível, e a confirmação de que
`CommitAsync` **não** é chamado quando a operação falha.

Testes em `test/DingFood.Tests/Application/Features/<Área>/`.

---

## 11. Checklist antes do PR

- [ ] Handler `internal sealed`, herdando `BaseCommandHandler`/`BaseQueryHandler`
- [ ] Escreve no banco → é Command (tem `IUnitOfWork`), não Query
- [ ] Uma pasta por caso de uso, nomeada pelo verbo
- [ ] Command/Query/Response são `sealed record`
- [ ] Campo novo em Response existente entrou como `{ get; init; }`, não posicional
- [ ] Mapping filtra `IsActive` em todos os níveis de coleção
- [ ] Escopo nomeado `CompanyId` / `...BranchId` para o `TenantRequestBehavior` agir
- [ ] Sem checagem manual de tenant no handler
- [ ] Sem `using` de EF Core, Infrastructure, API ou `Microsoft.AspNetCore.*`
- [ ] Data de regra vinda do `TimeProvider`, não `DateTime.Now`
- [ ] Validator escrito; se o command é montado na controller, validação também no handler
- [ ] Orquestração repetida extraída para `Features/<Área>/Shared/` e registrada no DI
- [ ] `dotnet test test/DingFood.ArchTests` verde
