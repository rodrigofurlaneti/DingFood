# AGENTS.md — DingFood.Tests

Regras do projeto de testes unitários. Complementa os `AGENTS.md` das camadas — leia o da camada
que você está testando para saber **o que** deve ser coberto; aqui está **como** se testa neste
repositório.

Os outros três projetos de teste têm propósitos distintos e não seguem estas regras:
`DingFood.ArchTests` (regras de arquitetura, NetArchTest), `DingFood.Specs` (BDD) e
`DingFood.E2ETests`.

---

## 1. Stack

```xml
xunit 2.9 · FluentAssertions 6.12 · NSubstitute 5.3
Microsoft.EntityFrameworkCore.Sqlite 9.0.2 · coverlet.collector 6.0
→ referencia os QUATRO projetos: Domain, Application, Infrastructure e API
```

Domain, Application e Infrastructure expõem `InternalsVisibleTo` para `DingFood.Tests` — por isso
handler `internal sealed` e repositório `internal sealed` são testáveis diretamente, sem truque.

```powershell
dotnet test test/DingFood.Tests
dotnet test test/DingFood.Tests --settings coverage.runsettings --collect:"XPlat Code Coverage"
```

**Mocking é NSubstitute**, não Moq. `Substitute.For<T>()`, `Arg.Any<T>()`, `Arg.Is<T>(...)`,
`.Returns(...)`, `.Received()` / `.DidNotReceive()`.

**Asserção é FluentAssertions**, não `Assert.*`. `x.Should().Be(...)`, `.BeTrue()`,
`.BeOfType<T>()`, `.BeCloseTo(...)`.

---

## 2. Estrutura — espelha `src/`

```
Domain/Entities/        um XTests.cs por entidade (~100 arquivos)
Application/
  Abstractions/
  Features/<Área>/      testes de handler
  DependencyInjectionTests.cs
Infrastructure/
  Persistence/
    Repositories/       + RepositoryTestBase.cs
    Configurations/     testes de mapeamento EF
  Integrations/<Provedor>/  + TestSupport/FakeHttpMessageHandler.cs
  Authentication/ Storage/
  DependencyInjectionTests.cs
API/
  Controllers/          + TestSupport/ControllerTestHelpers.cs
  CompanyContextMiddlewareTests.cs, WorkplaceContextMiddlewareTests.cs, ProgramTests.cs
AuditRegressionTests.cs      regressões de segurança/auditoria (raiz)
PaymentAvailabilityFixture.cs
```

Arquivo de teste vai no caminho equivalente ao do arquivo testado, com sufixo `Tests`.

⚠️ **Inconsistência existente:** `Domain/` tem arquivos soltos (`AppUserTests.cs`,
`CustomerOrderTests.cs`, `PromotionTests.cs`, `StockItemTests.cs`) **e** `Domain/Entities/` tem
arquivos com o mesmo nome e conteúdo diferente (o `AppUserTests.cs` solto tem 896 bytes; o de
`Entities/` tem 7.803). Só não colidem porque os namespaces diferem. **Coloque teste de entidade
em `Domain/Entities/`** — os soltos são resquício a consolidar.

---

## 3. Convenções de escrita

### Nome do teste

```
Metodo_Cenario_ShouldResultadoEsperado
```

```csharp
public void Create_WithValidArguments_ShouldReturnSuccessResultWithCorrectProperties()
public void Create_WithEmptyOrWhitespaceLegalName_ShouldReturnFailureResult()
public async Task GetByCompany_Success_ShouldSendQueryAndReturnOk()
public async Task GetByCompany_Failure_ShouldReturnMappedErrorResult()
```

### Arrange / Act / Assert com comentário

```csharp
[Fact]
public void Deactivate_ShouldUpdateIsActiveToFalseAndSetUpdatedAt()
{
    // Arrange
    var supplier = Supplier.Create(1, "Supplier Corp Ltd", null, null, null, null).Value;

    // Act
    supplier.Deactivate();

    // Assert
    supplier.IsActive.Should().BeFalse();
    supplier.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
}
```

Em teste curto (3 linhas), os comentários podem ser omitidos — é o que os testes de controller
fazem. Em teste de entidade, mantenha.

### `[Theory]` para entrada inválida

O trio nulo/vazio/espaço é padrão:

```csharp
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("   ")]
public void Create_WithEmptyOrWhitespaceLegalName_ShouldReturnFailureResult(string? invalid)
```

### Afirme sobre `Error.Code` **e** `Error.Message`

```csharp
result.IsSuccess.Should().BeFalse();
result.Error.Code.Should().Be("Supplier.EmptyLegalName");
result.Error.Message.Should().Be("LegalName is required.");
```

O `Code` é o que define o HTTP status (`backend/AGENTS.md` §4) — travá-lo em teste evita que
alguém o mude e altere o status da API sem perceber.

### Data

`BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1))`. Nunca compare igualdade exata de timestamp.

### Construtor privado do EF

Existe um padrão de cobertura para o construtor sem parâmetros:

```csharp
[Fact]
public void PrivateConstructor_ShouldBeCoveredViaReflection_ForORMSerialization()
{
    var instance = Activator.CreateInstance(typeof(Supplier), true) as Supplier;
    instance.Should().NotBeNull();
    instance!.IsActive.Should().BeFalse();
}
```

É teste de cobertura, não de comportamento. Replique só se estiver perseguindo métrica.

---

## 4. Testar entidade (Domain)

O mais barato: sem mock, sem banco, sem async.

Cubra por entidade: cada `return Result.Failure` do `Create`, cada transição de estado válida e
a inválida correspondente, `Touch`/`Deactivate`, e a imutabilidade da coleção de filhos em
agregado.

---

## 5. Testar handler (Application)

Substitua repositórios e serviços; o `ILogTrackerRepository` e o `IUnitOfWork` são sempre
necessários porque as bases os exigem.

```csharp
var repo = Substitute.For<ISupplierRepository>();
var log  = Substitute.For<ILogTrackerRepository>();
var uow  = Substitute.For<IUnitOfWork>();

var handler = new CreateSupplierCommandHandler(repo, log, uow);
var result = await handler.Handle(new CreateSupplierCommand(1, "", null, null, null, null), default);

result.IsFailure.Should().BeTrue();
await uow.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
```

**Sempre verifique que `CommitAsync` NÃO é chamado quando a operação falha.** É a asserção que
pega o handler que persiste antes de validar.

Fixture compartilhada para dependência recorrente — o padrão é uma classe `internal static` com
métodos de fábrica:

```csharp
internal static class PaymentAvailabilityFixture
{
    internal static IPaymentMethodAvailability Allowed() { ... }
}
```

---

## 6. Testar repositório — `RepositoryTestBase`

Herde de `RepositoryTestBase` e use `Context` e `TenantService`.

```csharp
public sealed class SupplierRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task GetByCompanyAsync_ShouldReturnOnlyActive()
    {
        TenantService.CompanyId = 1;
        // ... semear via Context, exercitar o repositório, afirmar
    }
}
```

O que a base faz e **por que** — não reinvente:

- **SQLite em memória real**, não o provider `InMemory` do EF. Motivo: o `InMemory` não avalia
  `HasQueryFilter`, e filtro de tenant é justamente o que precisa ser verificado.
- **Uma conexão por teste.** O xUnit cria uma instância nova da classe por `[Fact]`/`[Theory]`,
  então o isolamento é automático — não escreva limpeza entre testes.
- **`Foreign Keys=False`** na connection string. Sem isso, cada teste precisaria semear
  Company → Branch → JobTitle → Employee só para satisfazer FK irrelevante ao comportamento.
  Teste de repositório isola **uma** entidade; integridade referencial é assunto do schema.
- **`EnsureCreated()`** a partir do modelo real do `AppDbContext`, não de migration.
- **`SqliteCompatibleModelCustomizer`** neutraliza três incompatibilidades MySQL→SQLite:
  `HasDefaultValueSql("CURRENT_TIMESTAMP(6)")` (SQLite não parseia), `IsRowVersion()` (não existe
  equivalente; vira `ValueGenerated.Never`) e colunas `TimeSpan` (convertidas para ticks, porque
  o SQLite não ordena `TimeSpan`).
- **`FakeCurrentTenantService`** com `CompanyId` mutável — é assim que se testa isolamento entre
  empresas.

Se você adicionar mapeamento novo que o SQLite não suporta, o lugar de tratar é o
`SqliteCompatibleModelCustomizer`, com comentário explicando por quê.

---

## 7. Testar client de integração — `FakeHttpMessageHandler`

Nenhum teste da suíte comum toca rede.

```csharp
var handler = new FakeHttpMessageHandler();
handler.EnqueueJson(HttpStatusCode.OK, """{"taxId":"14486046000177", ...}""");

var http = new HttpClient(handler);
var client = new CnpjaClient(http, Options.Create(new CnpjaSettings()), NullLogger<CnpjaClient>.Instance);

var result = await client.GetOfficeAsync("14486046000177");

result.IsSuccess.Should().BeTrue();
handler.Requests[0].RequestUri!.AbsolutePath.Should().EndWith("/office/14486046000177");
```

O handler enfileira respostas na ordem (`Enqueue`, `EnqueueJson`), cai em
`DefaultResponseFactory` (200 vazio) quando a fila esvazia, e guarda `Requests` + `RequestBodies`
para asserção de método, URL e corpo.

**Cubra cada ramo de erro do client** e confirme o `Error.Code` de cada um: 404, 429, timeout,
5xx, corpo inválido. É o que garante que o contrato com a camada de cima não quebre.

---

## 8. Testar controller — `ControllerTestHelpers`

Todo controller herda de `ApiController`, cujo `ExecuteWithLogAsync` acessa
`HttpContext.User/Connection/RequestServices`. **Sem `HttpContext` anexado, o teste lança
`NullReferenceException` antes de chegar na lógica.**

```csharp
public sealed class SuppliersControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly SuppliersController _controller;

    public SuppliersControllerTests()
    {
        _controller = new SuppliersController(_mediator, _logRepository, _unitOfWork);
        ControllerTestHelpers.AttachHttpContext(_controller);   // obrigatório
    }

    [Fact]
    public async Task GetByCompany_Success_ShouldSendQueryAndReturnOk()
    {
        _mediator.Send(Arg.Is<GetSuppliersByCompanyQuery>(q => q.CompanyId == 1), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IReadOnlyCollection<SupplierResponse>>([]));

        var result = await _controller.GetByCompany(1, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }
}
```

`AttachHttpContext` aceita `services` e `userId` opcionais, para action que resolve serviço do
`RequestServices` ou lê claim.

Dois testes por action, no mínimo: sucesso → `OkObjectResult`/`NoContentResult`; falha → o tipo
de resultado que o `Error.Code` deve produzir. Use `Arg.Is<TCommand>(c => ...)` para provar que o
controller montou o command com os valores certos.

---

## 9. Testes que não são de unidade

### `DependencyInjectionTests`

Existem dois (`Application/` e `Infrastructure/`) e verificam que o container resolve os
serviços registrados. **É a rede que pega "esqueci de registrar no `DependencyInjection.cs`"** —
erro que só aparece em runtime. Serviço novo com contrato público merece uma linha aqui.

### `AuditRegressionTests` (raiz)

Regressões de segurança que não cabem numa camada: prova de leitura ligada a mesa/comanda/método
com expiração, e `[Theory]` verificando que o `FeatureAccessConvention` realmente anexa
`AuthorizeFilter` aos controllers. **Controller novo protegido por feature deveria entrar nesse
`[Theory]`.**

### Testes contra MySQL real — opt-in por variável de ambiente

Alguns arquivos (`BrandMySqlIntegrationTests`, `OwnDeliveryMySqlTests`,
`WorkplacePerformanceMySqlTests`) falam com um MySQL isolado. São **pulados por padrão** via
atributo customizado:

```csharp
public sealed class LocalMySqlFactAttribute : FactAttribute
{
    public LocalMySqlFactAttribute()
    {
        if (Environment.GetEnvironmentVariable("DINGFOOD_ISOLATED_MYSQL_TEST") != "1")
            Skip = "Requires the isolated MySQL instance on 127.0.0.1:33197 with dump DDL only.";
    }
}
```

O comentário no código é a regra: *"Opt-in, isolated server only. The ordinary suite never
connects to an application database."*

⚠️ **Jamais aponte teste para o banco de desenvolvimento ou de produção.** Se precisar de MySQL
real, siga esse padrão: atributo com `Skip`, variável de ambiente própria, instância isolada em
porta dedicada.

---

## 10. Cobertura e CI

`coverage.runsettings` na raiz de `backend/`. O job do CI roda as três suítes (`Tests`,
`ArchTests`, `Specs`) e envia cobertura ao SonarQube Cloud, com `**/Migrations/**` excluído.

Teste que falha derruba o build. Teste lento ou instável é pior que teste ausente — se depender
de tempo real, relógio ou rede, ele está no lugar errado.

---

## 11. Checklist antes do PR

- [ ] Arquivo em `<Camada>/<subpasta espelhando src>/XTests.cs`; entidade vai em `Domain/Entities/`
- [ ] Nome no formato `Metodo_Cenario_ShouldResultado`
- [ ] FluentAssertions e NSubstitute (não `Assert.*`, não Moq)
- [ ] Falha de negócio afirma `Error.Code` **e** `Error.Message`
- [ ] Handler: verificado que `CommitAsync` não roda no caminho de falha
- [ ] Repositório: herda `RepositoryTestBase`, sem limpeza manual entre testes
- [ ] Client HTTP: `FakeHttpMessageHandler`, com um teste por ramo de erro
- [ ] Controller: `ControllerTestHelpers.AttachHttpContext` no construtor
- [ ] Nenhum teste toca rede ou banco de aplicação
- [ ] Teste que exige MySQL real usa atributo com `Skip` + variável de ambiente
- [ ] Serviço novo no DI ganhou linha em `DependencyInjectionTests`
- [ ] Data comparada com `BeCloseTo`, nunca igualdade exata
