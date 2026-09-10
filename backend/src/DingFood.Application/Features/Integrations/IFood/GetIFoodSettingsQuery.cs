using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood;

public sealed record GetIfoodSettingsQuery(long CompanyId) : IQuery<IfoodSettingsResponse>;
