using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.DeactivateComplementItem;

public sealed record DeactivateComplementItemCommand(long ComplementItemId) : ICommand;
