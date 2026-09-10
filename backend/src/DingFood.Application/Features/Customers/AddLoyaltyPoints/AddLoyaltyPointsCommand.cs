using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Customers.AddLoyaltyPoints;

public sealed record AddLoyaltyPointsCommand(long CustomerId, int Points) : ICommand;
