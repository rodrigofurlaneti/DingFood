using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Access.SetUserFeatures;

public sealed record SetUserFeaturesCommand(
    long AppUserId,
    IReadOnlyCollection<long> FeatureIds) : ICommand;
