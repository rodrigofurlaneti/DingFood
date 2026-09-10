using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Access.GetJobTitleFeatures;

public sealed record GetJobTitleFeaturesQuery(long JobTitleId) : IQuery<IReadOnlyCollection<long>>;
