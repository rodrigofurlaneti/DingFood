using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.ServiceFeeSetting;

public sealed record GetServiceFeeSettingQuery(long BranchId) : IQuery<ServiceFeeSettingResponse>;
