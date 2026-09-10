using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Promotions.Create;

public sealed record CreatePromotionCommand(
    long BranchId,
    long ProductId,
    string Name,
    int DayOfWeek,
    int StartMinuteOfDay,
    int EndMinuteOfDay,
    long PromotionTypeId,
    decimal? DiscountRate) : ICommand<long>;
