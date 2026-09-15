# AGENTS.md — DingFood.Domain

Regras específicas da camada de domínio. Complementa o `backend/AGENTS.md` — leia aquele primeiro
para arquitetura geral, multi-tenancy, persistência e fluxo de feature. Aqui só o que é próprio
do Domain.

---

## 1. O que este projeto é

Uma class library **pura**. Sem NuGet nenhum além do SDK:

```xml
<TargetFramework>net9.0</TargetFramework>
<Nullable>enable</Nullable>
<LangVersion>13</LangVersion>
<InternalsVisibleTo Include="DingFood.Tests" />
<InternalsVisibleTo Include="DingFood.Specs" />
```

O teste `Domain_ShouldNotDependOn_OuterLayersOrFrameworks` proíbe referência a Application,
Infrastructure, API, **MediatR, EF Core e FluentValidation**. Se você precisou de um pacote aqui,
quase certamente a lógica pertence a outra camada.

Consequências práticas:

- Sem `[Required]`, `[MaxLength]` ou qualquer DataAnnotation de EF. Mapeamento é
  `IEntityTypeConfiguration` na Infrastructure.
- Sem atributo de validação. Invariante se defende no construtor / método de fábrica.
- Sem `async`/`Task` em regra de negócio. O domínio é síncrono; I/O é das outras camadas.

```
Domain/
  Constants/     lookups fixos e validadores puros
  Entities/      ~125 entidades
  Enums/         praticamente vazio (só WebhookLogStatus) — ver §7
  Exceptions/    3 exceções
  Primitives/    Entity, AggregateRoot, Result, Error, ValueObject, IDomainEvent
  Repositories/  ~85 interfaces + IUnitOfWork
```

---

## 2. Primitives

| Tipo | Papel |
|---|---|
| `Entity` | `long Id` com `protected set`, igualdade por identidade (`Id != 0`) |
| `AggregateRoot : Entity` | raiz de consistência; carrega lista de domain events |
| `Result` / `Result<T>` | retorno de operação que pode falhar |
| `Error(Code, Message)` | record; `Error.None`, `Error.NullValue` e fábricas `Validation`/`NotFound`/`Conflict`/`Failure` |
| `ValueObject` | base abstrata com igualdade estrutural por `GetAtomicValues()` |
| `IDomainEvent` | interface marcadora |

`Result` lança `InvalidOperationException` se você montar um sucesso com erro, um fracasso sem
erro, ou ler `.Value` de um fracasso. Sempre teste `IsFailure` antes de acessar `.Value`.

---

## 3. Três formatos de entidade — saiba qual você está escrevendo

### 3.1 Agregado rico (o padrão a seguir)

Raiz de consistência com repositório próprio. Encapsula filhos e expõe comportamento.
Referência: `CustomerOrder` (21 KB, ~25 métodos de negócio), `CashSession`, `Sale`, `StockItem`.

```csharp
public sealed class CustomerOrder : AggregateRoot
{
    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private CustomerOrder() : base(0) { }                 // EF

    public static Result<CustomerOrder> Create(...) { ... }

    public Result AddItem(long productId, decimal unitPrice, decimal quantity, ...,
                          DateTime Now, ...) { ... }
    public Result Close(decimal serviceFeeRate, DateTime Now) { ... }
    public Result Cancel(DateTime Now) { ... }
}
```

Regras:

- **Coleção de filhos é `private readonly List<T>` exposta como `IReadOnlyCollection<T>` via
  `AsReadOnly()`.** Nunca exponha `List<T>` nem `ICollection<T>` — isso deixaria o handler
  adicionar item por fora, furando as invariantes.
- **Método que pode falhar devolve `Result`**, não `void` e não exceção.
- Filho só é criado e modificado por método da raiz (`AddItem`, `UpdateItemStatus`,
  `AddComplement`, `RemoveComplement`).
- Um agregado pode criar outro no construtor quando a existência é obrigatória —
  `Company` cria seu `BusinessGroup`. Use com parcimônia.

### 3.2 Entidade filha

`OrderItem`, `SalePayment`, `CashMovement`, `StockMovement`, `PurchaseItem`.

Herda **`Entity`**, não `AggregateRoot` — e há teste de arquitetura que verifica isso. Não tem
repositório próprio: é carregada junto da raiz.

### 3.3 Lookup / tabela de referência

`OrderStatus`, `OrderItemStatus`, `TableStatus`, `ComandaStatus`, `CashSessionStatus`,
`CashMovementType`, `StockMovementType`, `PaymentMethod`, `CostType`, `UnitOfMeasure`,
`ShiftClosingStatus`, `Permission`, `AppFeature`.

Herda `Entity`. Estrutura mínima: `Name`, auditoria, `Create` → `Result<T>`, `Touch()`,
`Deactivate()`. Os **Ids são fixos**, seedados no banco, e ficam em `Constants/LookupIds.cs`:

```csharp
public static class OrderStatusIds
{
    public const long Aberto = 1;
    public const long EmAndamento = 2;
    public const long AguardandoPagamento = 3;
    public const long Pago = 4;
    public const long Cancelado = 5;
    public const long WebSite = 6;
}
```

⚠️ **Nunca altere um valor existente em `LookupIds`.** Ele espelha `BarRestaurante_Seed.sql`;
mudar o número quebra todos os registros já gravados. Para adicionar, use o próximo número livre
**e** atualize o seed.

Compare sempre por constante, nunca por literal ou por nome:

```csharp
if (order.OrderStatusId == OrderStatusIds.Aberto)      // ✅
if (order.OrderStatusId == 1)                          // ❌
if (status.Name == "Aberto")                           // ❌
```

---

## 4. Anatomia obrigatória de uma entidade

```csharp
using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class Supplier : AggregateRoot          // sempre sealed (há teste)
{
    public long CompanyId { get; private set; }        // setter privado
    public string LegalName { get; private set; } = null!;
    public string? Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Supplier() : base(0) { }                   // EF materializa por aqui
    private Supplier(long companyId, string legalName, ...) : base(0) { ... }

    public static Result<Supplier> Create(long companyId, string legalName, ...)
    {
        if (string.IsNullOrWhiteSpace(legalName))
            return Result.Failure<Supplier>(new Error("Supplier.EmptyLegalName", "LegalName is required."));

        return Result.Success(new Supplier(companyId, legalName, ...));
    }

    public void Touch() => UpdatedAt = DateTime.Now;

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.Now;
    }
}
```

Checklist:

- [ ] `sealed`
- [ ] Todo setter `private set` (exceções em §8)
- [ ] `private` ctor sem parâmetros para o EF (`base(0)`)
- [ ] `private` ctor com parâmetros, chamado só pela fábrica
- [ ] `static Create(...)` devolvendo `Result<T>`, validando invariantes
- [ ] `CreatedAt` / `UpdatedAt?` / `IsActive`
- [ ] `Deactivate()` — exclusão é lógica, nunca física
- [ ] `string` não-nulo inicializado com `= null!` (o EF preenche)

### Construtor grande

Passando de ~10 parâmetros, declare um `record` de snapshot **no mesmo arquivo** e passe-o a
`Create` e a um `Refresh`. Referência: `CnpjQuerySnapshot` / `CepQuerySnapshot`.

```csharp
public sealed record CepQuerySnapshot(string? Street, string? District, ..., string RawJson);

public static Result<CepQuery> Create(string cep, CepQuerySnapshot snapshot) { ... }
public void Refresh(CepQuerySnapshot snapshot) { ... }
```

Isso também evita o erro de trocar dois parâmetros `string?` de posição.

### Error.Code define o HTTP status

`<Entidade>.<Motivo>` em PascalCase. Sufixos `.NotFound`, `.AlreadyExists`, `.Duplicate` e
`.Forbidden` viram 404/409/409/403 automaticamente; qualquer outro vira 400. Detalhe no
`backend/AGENTS.md` §4.

---

## 5. Tempo — há duas convenções no código

**Agregado rico recebe o instante por parâmetro:**

```csharp
public Result Close(decimal serviceFeeRate, DateTime Now) { ... }
public Result MarkAsPaid(DateTime Now) { ... }
```

Isso torna a regra testável sem congelar relógio. A Infrastructure registra
`TimeProvider` → `TimeProviderCustom`, que é de onde o instante deve vir.

**Entidade simples usa `DateTime.Now` direto** no `Create`/`Touch`/`Deactivate`.

Ao escrever código novo: se a data participa de **regra** (prazo, fechamento, expiração,
vigência), **receba por parâmetro**. Se é só carimbo de auditoria, `DateTime.Now` está de acordo
com o que existe.

⚠️ O projeto usa `DateTime.Now` (hora local), **não `UtcNow`**. Não misture os dois na mesma
entidade. Existe um `UtcDateTimeConverter` na serialização da API.

---

## 6. Repositórios — só a interface mora aqui

`Domain/Repositories/IXRepository.cs`. A implementação é `internal sealed` na Infrastructure
(há teste de arquitetura para as duas coisas).

Vocabulário consolidado:

| Método | Uso |
|---|---|
| `GetByIdAsync` | leitura, sem tracking |
| `GetByIdForUpdateAsync` | leitura **com** tracking, para alterar e commitar |
| `GetByCompanyAsync` / `GetByBranchAsync` | listagem escopada |
| `GetOpenBy<X>ForUpdateAsync` | busca de agregado em estado específico, para alteração |
| `Has<X>Async` | existência, devolve `bool` — não traga a entidade só para checar |
| `AddAsync` | inserção |

O par `GetById` / `GetByIdForUpdate` **não é redundante** — é o que evita tracking desnecessário
em consulta e garante tracking em escrita.

Não existe `UpdateAsync` nem `DeleteAsync`. Atualização acontece por rastreamento do EF sobre a
entidade retornada por `...ForUpdateAsync`; exclusão é `Deactivate()`. **Não crie esses métodos.**

`IUnitOfWork` expõe só `CommitAsync`. O repositório nunca commita — quem commita é o handler.

---

## 7. Scaffolding presente mas não usado

Duas peças existem em `Primitives` e **não têm uso no modelo hoje**. Saiba disso antes de
assumir que "o projeto usa":

**Domain events.** `AggregateRoot` tem `RaiseDomainEvent`, `GetDomainEvents` e
`ClearDomainEvents`, e existe `IDomainEvent`. Mas `AppDbContext.CommitAsync` só chama
`SaveChangesAsync` — **não há despachante**. Um evento levantado hoje é acumulado na lista e
descartado. Se for usar, o despacho precisa ser implementado antes; efeito colateral entre
agregados hoje é feito no handler.

**ValueObject.** A base existe, mas não encontrei entidade que a herde — o modelo é de
propriedades primitivas (`string Cnpj`, `string? Email`). Introduzir um VO exige mapeamento
`OwnsOne` na Configuration, então não é mudança só de domínio.

**Enums.** A pasta tem um único arquivo (`WebhookLogStatus`). O padrão do projeto para estado é
**tabela de lookup + `LookupIds`**, não `enum` C#. Siga o lookup.

---

## 8. Exceções conhecidas ao padrão

Não replique, mas saiba que existem:

- **`LogTracker`** — construtor público, setters públicos, sem fábrica, sem validação. É registro
  de auditoria escrito por `BaseCommandHandler`/`ApiController`, não modelo de negócio.
- **`Company`** — tem navegação `public BusinessGroup BusinessGroup` e a cria no construtor. A
  maioria das entidades usa FK sombra (`HasOne<X>().WithMany().HasForeignKey(...)` na
  Configuration) e **não** declara propriedade de navegação.
- **Nome de arquivo x nome de classe** — `IFoodIntegrationSetting.cs` contém
  `IfoodIntegrationSetting`; `IFoodOrder.cs` contém `IFoodOrder`. A inconsistência é antiga. Ao
  criar arquivo novo, faça bater; ao mexer em existente, siga o nome da classe.
- **`ConcurrencyException`** não é `sealed` e usa o estilo antigo de namespace com chaves.

---

## 9. Exceções do domínio

Três, todas com semântica específica:

| Exceção | Quem lança | Efeito |
|---|---|---|
| `TenantAccessException` | `TenantRequestBehavior` | HTTP **403** |
| `ConcurrencyException` | `AppDbContext.CommitAsync` | envolve `DbUpdateException`/`DbUpdateConcurrencyException` |
| `DeliveryPricingException` | cálculo de frete | — |

**Regra de negócio não lança exceção — devolve `Result.Failure`.** Exceção aqui é para condição
que o chamador não tem como tratar localmente.

---

## 10. Testar domínio

Domínio é a camada mais barata de testar: sem mock, sem banco, sem async.

```csharp
[Fact]
public void Create_ShouldFail_WhenLegalNameIsEmpty()
{
    var result = Supplier.Create(1, "  ", null, null, null, null);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Supplier.EmptyLegalName");
}
```

Cubra, para cada entidade nova: cada `return Result.Failure` do `Create`, cada transição de
estado válida e a inválida correspondente, e a imutabilidade da coleção de filhos.

Testes em `test/DingFood.Tests/Domain/`. `InternalsVisibleTo` já libera `DingFood.Tests` e
`DingFood.Specs`.

---

## 11. Checklist antes do PR

- [ ] `sealed`, ctor privado para EF, fábrica `Create` → `Result<T>`
- [ ] Setters privados; coleção de filhos exposta como `IReadOnlyCollection<T>`
- [ ] `CreatedAt` / `UpdatedAt?` / `IsActive` + `Deactivate()`
- [ ] Método que pode falhar devolve `Result`, não lança
- [ ] `Error.Code` no formato `<Entidade>.<Motivo>`, com o sufixo que produz o HTTP certo
- [ ] Agregado herda `AggregateRoot`; filho herda `Entity`
- [ ] Nenhum `using` de MediatR, EF Core ou FluentValidation
- [ ] Lookup novo: constante em `LookupIds` **e** linha no seed SQL
- [ ] Data que participa de regra recebida por parâmetro, não `DateTime.Now` inline
- [ ] Interface do repositório aqui; implementação na Infrastructure
- [ ] Entidade classificada para multi-tenancy (`backend/AGENTS.md` §6) — senão a migration nem gera
- [ ] `dotnet test test/DingFood.ArchTests` verde
