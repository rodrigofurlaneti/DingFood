using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood;

public sealed record SetIfoodMerchantMappingCommand(long BranchId, string? MerchantId, string? MerchantUuid) : ICommand;
