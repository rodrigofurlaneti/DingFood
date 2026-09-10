using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.DeactivateComplementGroup;

public sealed record DeactivateComplementGroupCommand(long ComplementGroupId) : ICommand;
