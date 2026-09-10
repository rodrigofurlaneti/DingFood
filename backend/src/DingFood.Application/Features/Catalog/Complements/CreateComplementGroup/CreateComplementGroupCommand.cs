using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.CreateComplementGroup;

public sealed record CreateComplementGroupCommand(
    long CompanyId,
    string Name,
    long ComplementGroupTypeId,
    int MinSelection,
    int MaxSelection) : ICommand<long>;
