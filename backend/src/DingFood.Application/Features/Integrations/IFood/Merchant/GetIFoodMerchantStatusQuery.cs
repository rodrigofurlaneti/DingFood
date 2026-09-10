using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

public sealed record GetIfoodMerchantStatusQuery(long BranchId) : IQuery<IfoodMerchantStatusResponse>;
