using DingFood.Domain.Entities;

namespace DingFood.Infrastructure.Printing;

internal interface IRawPrinterTransport
{
    bool CanHandle(Printer printer);
    Task SendAsync(Printer printer, byte[] payload, CancellationToken cancellationToken);
}
