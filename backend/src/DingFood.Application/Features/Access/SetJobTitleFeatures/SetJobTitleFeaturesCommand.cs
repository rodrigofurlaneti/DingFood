using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;

namespace DingFood.Application.Features.Access.SetJobTitleFeatures;

public sealed record SetJobTitleFeaturesCommand(long JobTitleId, List<long> FeatureIds) : ICommand<Result>;