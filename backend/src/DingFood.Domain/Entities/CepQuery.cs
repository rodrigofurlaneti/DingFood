using DingFood.Domain.Constants;
using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

/// <summary>
/// Dados de endereço devolvidos pela consulta de CEP, usados tanto para criar
/// quanto para atualizar um <see cref="CepQuery"/>.
/// </summary>
public sealed record CepQuerySnapshot(
    string? Street,
    string? Complement,
    string? Unit,
    string? District,
    string? City,
    string? StateAbbreviation,
    string? StateName,
    string? Region,
    int? IbgeCode,
    string? GiaCode,
    string? AreaCode,
    string? SiafiCode,
    string RawJson);

/// <summary>
/// Snapshot de uma consulta de CEP na base dos Correios (via ViaCEP).
///
/// Mesma estratégia do <see cref="CnpjQuery"/>: campos consultáveis em colunas próprias e a
/// resposta original em <see cref="RawJson"/>.
///
/// Tabela GLOBAL: um CEP é o mesmo para qualquer empresa da instalação, portanto sem CompanyId
/// e declarada em AppDbContext.OperationalScopes.SystemTables. É esse compartilhamento que
/// mantém o volume de chamadas ao ViaCEP baixo — o serviço bloqueia acesso por uso massivo.
/// </summary>
public sealed class CepQuery : AggregateRoot
{
    // ---------- Identificação ----------
    public string Cep { get; private set; } = null!;              // char(8), único, somente dígitos

    // ---------- Endereço ----------
    public string? Street { get; private set; }                   // logradouro
    public string? Complement { get; private set; }               // complemento — ex.: "lado ímpar"
    public string? Unit { get; private set; }                     // unidade (CEP de grande usuário)
    public string? District { get; private set; }                 // bairro
    public string? City { get; private set; }                     // localidade
    public string? StateAbbreviation { get; private set; }        // uf — "SP"
    public string? StateName { get; private set; }                // estado — "São Paulo"
    public string? Region { get; private set; }                   // regiao — "Sudeste"

    // ---------- Códigos oficiais ----------
    public int? IbgeCode { get; private set; }                    // código do município — 3550308
    public string? GiaCode { get; private set; }                  // GIA/ICMS, só existe para SP
    public string? AreaCode { get; private set; }                 // ddd — "11"
    public string? SiafiCode { get; private set; }                // código SIAFI do município

    // ---------- Payload e controle ----------
    public string RawJson { get; private set; } = null!;          // longtext — resposta original
    public DateTime QueriedAt { get; private set; }               // base do TTL de cache
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private CepQuery() : base(0) { }

    private CepQuery(string cep) : base(0)
    {
        Cep = cep;
        IsActive = true;
        CreatedAt = DateTime.Now;
    }

    /// <summary>Cria o registro a partir de um CEP (com ou sem hífen) e do snapshot da consulta.</summary>
    public static Result<CepQuery> Create(string cep, CepQuerySnapshot snapshot)
    {
        var normalized = CepValidator.Normalize(cep);

        if (!CepValidator.IsValid(normalized))
            return Result.Failure<CepQuery>(new Error("Cep.Invalid", "O CEP informado é inválido."));

        if (snapshot is null)
            return Result.Failure<CepQuery>(new Error("Cep.EmptySnapshot", "Os dados da consulta são obrigatórios."));

        if (string.IsNullOrWhiteSpace(snapshot.RawJson))
            return Result.Failure<CepQuery>(new Error("Cep.EmptyPayload", "O retorno da consulta está vazio."));

        // Logradouro vem vazio em CEP de cidade inteira (ex.: 01000-000) — isso é legítimo,
        // então só a cidade é exigida para considerar a resposta aproveitável.
        if (string.IsNullOrWhiteSpace(snapshot.City))
            return Result.Failure<CepQuery>(new Error("Cep.EmptyCity", "A consulta não retornou a cidade do CEP."));

        var entity = new CepQuery(normalized);
        entity.Apply(snapshot);
        entity.QueriedAt = DateTime.Now;

        return Result.Success(entity);
    }

    /// <summary>Atualiza o registro com uma consulta nova, reiniciando a janela de cache.</summary>
    public void Refresh(CepQuerySnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        Apply(snapshot);
        QueriedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        IsActive = true;
    }

    /// <summary>TTL menor ou igual a zero desliga o cache (sempre stale).</summary>
    public bool IsStale(int ttlDays) => ttlDays <= 0 || QueriedAt < DateTime.Now.AddDays(-ttlDays);

    public int AgeInDays => Math.Max(0, (int)(DateTime.Now - QueriedAt).TotalDays);

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.Now;
    }

    private void Apply(CepQuerySnapshot snapshot)
    {
        Street = Trim(snapshot.Street);
        Complement = Trim(snapshot.Complement);
        Unit = Trim(snapshot.Unit);
        District = Trim(snapshot.District);
        City = Trim(snapshot.City);
        StateAbbreviation = Trim(snapshot.StateAbbreviation)?.ToUpperInvariant();
        StateName = Trim(snapshot.StateName);
        Region = Trim(snapshot.Region);
        IbgeCode = snapshot.IbgeCode;
        GiaCode = Trim(snapshot.GiaCode);
        AreaCode = Trim(snapshot.AreaCode);
        SiafiCode = Trim(snapshot.SiafiCode);
        RawJson = snapshot.RawJson;
    }

    // O ViaCEP usa string vazia — e não null — para campo sem valor ("unidade": "", "gia": "").
    private static string? Trim(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
