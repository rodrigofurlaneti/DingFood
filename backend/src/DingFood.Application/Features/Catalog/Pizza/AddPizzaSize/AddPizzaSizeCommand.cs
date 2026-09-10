using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Pizza.AddPizzaSize;

public sealed record AddPizzaSizeCommand(
    long PizzaConfigurationId,
    string Name,
    int? Slices,
    int AcceptedFractions,
    int DisplayOrder) : ICommand<long>;
