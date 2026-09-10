using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Pizza.AddPizzaEdge;

public sealed record AddPizzaEdgeCommand(
    long PizzaConfigurationId,
    string Name,
    decimal ExtraPrice,
    int DisplayOrder) : ICommand<long>;
