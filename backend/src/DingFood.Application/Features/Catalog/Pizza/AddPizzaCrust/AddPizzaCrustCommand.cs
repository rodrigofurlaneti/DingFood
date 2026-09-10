using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Pizza.AddPizzaCrust;

public sealed record AddPizzaCrustCommand(
    long PizzaConfigurationId,
    string Name,
    decimal ExtraPrice,
    int DisplayOrder) : ICommand<long>;
