using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Access.GetFeatures;

public sealed record GetFeaturesQuery : IQuery<IReadOnlyCollection<FeatureResponse>>;
