using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.UpdateComplementPrice;

public sealed record UpdateComplementPriceCommand(long ComplementGroupId, long ComplementId, decimal ExtraPrice) : ICommand;
