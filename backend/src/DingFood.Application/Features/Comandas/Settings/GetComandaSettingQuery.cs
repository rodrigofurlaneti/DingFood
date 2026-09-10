using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Comandas.Settings;

public sealed record GetComandaSettingQuery(long BranchId) : IQuery<ComandaSettingResponse>;
