namespace DingFood.Application.Abstractions.Tenancy;

public interface IPublicWorkplaceScope
{
    Task BindAsync(long? branchId, Guid? tableToken, long? orderId, CancellationToken ct);
}
