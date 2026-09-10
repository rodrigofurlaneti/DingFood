using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.UpdateComplementGroup;

public sealed record UpdateComplementGroupCommand(
    long ComplementGroupId,
    string Name,
    long ComplementGroupTypeId,
    int MinSelection,
    int MaxSelection) : ICommand;
