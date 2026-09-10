using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Promotions.Deactivate;

public sealed record DeactivatePromotionCommand(long PromotionId) : ICommand;
