using DingFood.Domain.Primitives;
namespace DingFood.Application.Abstractions.Notifications;

public interface IWhatsAppQueue
{
    Task<Result> EnqueueAsync(string phoneNumber, string message, string fileUrl, CancellationToken cancellationToken = default);
}
