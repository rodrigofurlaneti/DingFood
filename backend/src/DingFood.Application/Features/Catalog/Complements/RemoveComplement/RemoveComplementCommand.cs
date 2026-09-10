using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.RemoveComplement;

public sealed record RemoveComplementCommand(long ComplementGroupId, long ComplementId) : ICommand;
