using DingFood.Domain.Constants;
using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

/// <summary>
/// Conjunto de dados cadastrais devolvidos pela consulta de CNPJ, usado tanto para criar
/// quanto para atualizar um <see cref="CnpjQuery"/> sem espalhar um método de 30 parâmetros.
/// </summary>
public sealed record CnpjQuerySnapshot(
    string LegalName,
    string? TradeName,
    DateOnly? FoundedOn,
    bool IsHeadOffice,
    int? StatusId,
    string? StatusText,
    DateOnly? StatusDate,
    string? ReasonText,
    int? NatureId,
    string? NatureText,
    string? SizeAcronym,
    string? SizeText,
    decimal? Equity,
    bool? SimplesOptant,
    DateOnly? SimplesSince,
    bool? SimeiOptant,
    DateOnly? SimeiSince,
    int? MainActivityCode,
    string? MainActivityText,
    string? AddressStreet,
    string? AddressNumber,
    string? AddressDetails,
    string? AddressDistrict,
    string? AddressCity,
    string? AddressState,
    string? AddressZip,
    int? MunicipalityCode,
    string? PrimaryPhone,
    string? PrimaryEmail,
    DateTime? SourceUpdatedAt,
    string RawJson);

/// <summary>
/// Snapshot de uma consulta de CNPJ na base pública da Receita Federal (via CNPJá).
///
/// Estratégia de persistência: os campos mais consultados ficam em colunas próprias e indexáveis,
/// e a resposta original completa (sócios, CNAEs secundários, telefones, e-mails, SUFRAMA) fica
/// em <see cref="RawJson"/>. Assim nada se perde quando o fornecedor evoluir o contrato.
///
/// Tabela GLOBAL: dados públicos da Receita não pertencem a nenhum tenant, portanto esta entidade
/// NÃO tem CompanyId e NÃO recebe query filter no AppDbContext. Uma consulta feita por uma empresa
/// aproveita para todas as demais — o que é exatamente o que protege a cota de 5 req/min da API.
/// </summary>
public sealed class CnpjQuery : AggregateRoot
{
    // ---------- Identificação ----------
    public string TaxId { get; private set; } = null!;              // char(14), único, somente dígitos
    public string LegalName { get; private set; } = null!;          // razão social
    public string? TradeName { get; private set; }                  // "alias" / nome fantasia
    public DateOnly? FoundedOn { get; private set; }                // "founded"
    public bool IsHeadOffice { get; private set; }                  // "head" — matriz (true) ou filial

    // ---------- Situação cadastral ----------
    public int? StatusId { get; private set; }                      // ex.: 4
    public string? StatusText { get; private set; }                 // ex.: "Inapta"
    public DateOnly? StatusDate { get; private set; }
    public string? ReasonText { get; private set; }                 // ex.: "Omissão de declarações"

    // ---------- Natureza jurídica e porte ----------
    public int? NatureId { get; private set; }                      // ex.: 2062
    public string? NatureText { get; private set; }                 // "Sociedade Empresária Limitada"
    public string? SizeAcronym { get; private set; }                // "ME"
    public string? SizeText { get; private set; }                   // "Microempresa"
    public decimal? Equity { get; private set; }                    // capital social — ex.: 46000

    // ---------- Simples Nacional / MEI ----------
    public bool? SimplesOptant { get; private set; }
    public DateOnly? SimplesSince { get; private set; }
    public bool? SimeiOptant { get; private set; }
    public DateOnly? SimeiSince { get; private set; }

    // ---------- Atividade principal (CNAE) ----------
    public int? MainActivityCode { get; private set; }              // ex.: 6209100
    public string? MainActivityText { get; private set; }

    // ---------- Endereço ----------
    public string? AddressStreet { get; private set; }
    public string? AddressNumber { get; private set; }
    public string? AddressDetails { get; private set; }             // complemento
    public string? AddressDistrict { get; private set; }            // bairro
    public string? AddressCity { get; private set; }
    public string? AddressState { get; private set; }               // UF
    public string? AddressZip { get; private set; }                 // CEP, somente dígitos
    public int? MunicipalityCode { get; private set; }              // código IBGE — ex.: 3518800

    // ---------- Contato principal ----------
    public string? PrimaryPhone { get; private set; }               // "1147889520"
    public string? PrimaryEmail { get; private set; }

    // ---------- Payload e controle ----------
    public DateTime? SourceUpdatedAt { get; private set; }          // campo "updated" da CNPJá
    public string RawJson { get; private set; } = null!;            // longtext — resposta original
    public DateTime QueriedAt { get; private set; }                 // base do TTL de cache
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private CnpjQuery() : base(0) { }

    private CnpjQuery(string taxId) : base(0)
    {
        TaxId = taxId;
        IsActive = true;
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Cria o registro a partir de um CNPJ (com ou sem pontuação) e do snapshot devolvido pela consulta.
    /// </summary>
    public static Result<CnpjQuery> Create(string taxId, CnpjQuerySnapshot snapshot)
    {
        var normalized = CnpjValidator.Normalize(taxId);

        if (!CnpjValidator.IsValid(normalized))
            return Result.Failure<CnpjQuery>(new Error("Cnpj.Invalid", "O CNPJ informado é inválido."));

        if (snapshot is null)
            return Result.Failure<CnpjQuery>(new Error("Cnpj.EmptySnapshot", "Os dados da consulta são obrigatórios."));

        if (string.IsNullOrWhiteSpace(snapshot.LegalName))
            return Result.Failure<CnpjQuery>(new Error("Cnpj.EmptyLegalName", "A razão social é obrigatória."));

        if (string.IsNullOrWhiteSpace(snapshot.RawJson))
            return Result.Failure<CnpjQuery>(new Error("Cnpj.EmptyPayload", "O retorno da consulta está vazio."));

        var entity = new CnpjQuery(normalized);
        entity.Apply(snapshot);
        entity.QueriedAt = DateTime.Now;

        return Result.Success(entity);
    }

    /// <summary>
    /// Atualiza o registro existente com uma consulta nova, reiniciando a janela de cache.
    /// </summary>
    public void Refresh(CnpjQuerySnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        Apply(snapshot);
        QueriedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        IsActive = true;
    }

    /// <summary>
    /// Indica se o registro já passou da janela de cache e precisa ser reconsultado.
    /// TTL menor ou igual a zero desliga o cache (sempre stale).
    /// </summary>
    public bool IsStale(int ttlDays) => ttlDays <= 0 || QueriedAt < DateTime.Now.AddDays(-ttlDays);

    /// <summary>Idade do dado em dias, para exibição.</summary>
    public int AgeInDays => Math.Max(0, (int)(DateTime.Now - QueriedAt).TotalDays);

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.Now;
    }

    private void Apply(CnpjQuerySnapshot snapshot)
    {
        LegalName = snapshot.LegalName.Trim();
        TradeName = Trim(snapshot.TradeName);
        FoundedOn = snapshot.FoundedOn;
        IsHeadOffice = snapshot.IsHeadOffice;

        StatusId = snapshot.StatusId;
        StatusText = Trim(snapshot.StatusText);
        StatusDate = snapshot.StatusDate;
        ReasonText = Trim(snapshot.ReasonText);

        NatureId = snapshot.NatureId;
        NatureText = Trim(snapshot.NatureText);
        SizeAcronym = Trim(snapshot.SizeAcronym);
        SizeText = Trim(snapshot.SizeText);
        Equity = snapshot.Equity;

        SimplesOptant = snapshot.SimplesOptant;
        SimplesSince = snapshot.SimplesSince;
        SimeiOptant = snapshot.SimeiOptant;
        SimeiSince = snapshot.SimeiSince;

        MainActivityCode = snapshot.MainActivityCode;
        MainActivityText = Trim(snapshot.MainActivityText);

        AddressStreet = Trim(snapshot.AddressStreet);
        AddressNumber = Trim(snapshot.AddressNumber);
        AddressDetails = Trim(snapshot.AddressDetails);
        AddressDistrict = Trim(snapshot.AddressDistrict);
        AddressCity = Trim(snapshot.AddressCity);
        AddressState = Trim(snapshot.AddressState)?.ToUpperInvariant();
        AddressZip = Trim(snapshot.AddressZip);
        MunicipalityCode = snapshot.MunicipalityCode;

        PrimaryPhone = Trim(snapshot.PrimaryPhone);
        PrimaryEmail = Trim(snapshot.PrimaryEmail)?.ToLowerInvariant();

        SourceUpdatedAt = snapshot.SourceUpdatedAt;
        RawJson = snapshot.RawJson;
    }

    private static string? Trim(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
