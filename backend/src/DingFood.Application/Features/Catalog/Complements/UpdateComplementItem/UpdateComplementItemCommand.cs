using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.UpdateComplementItem;

public sealed record UpdateComplementItemCommand(long ComplementItemId, string Name) : ICommand;
