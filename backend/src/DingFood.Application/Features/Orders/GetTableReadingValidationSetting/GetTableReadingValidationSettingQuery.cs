using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.GetTableReadingValidationSetting;

public sealed record GetTableReadingValidationSettingQuery(long BranchId) : IQuery<TableReadingValidationSettingResponse>;
