using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Access.GetUserFeatures;

public sealed record GetUserFeaturesQuery(long AppUserId) : IQuery<IReadOnlyCollection<long>>;
