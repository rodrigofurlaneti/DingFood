using Microsoft.AspNetCore.Authorization;

namespace DingFood.API.Authorization;

public sealed class FeatureRequirement(string featureCode) : IAuthorizationRequirement
{
    public string FeatureCode { get; } = featureCode;
}
