using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.CreateComplementItem;

// Fase 18 (combos) — LinkedProductId é opcional: quando informado, este item de complemento
// aponta pra um Product real do cardápio (mesma imagem/estoque) em vez de ser só um texto solto —
// ver comentário completo em ComplementItem.
public sealed record CreateComplementItemCommand(long CompanyId, string Name, long? LinkedProductId = null) : ICommand<long>;
