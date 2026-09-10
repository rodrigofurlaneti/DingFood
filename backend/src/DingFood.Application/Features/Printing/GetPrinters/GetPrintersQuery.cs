using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Printing.GetPrinters;

public sealed record GetPrintersQuery(long BranchId) : IQuery<IReadOnlyCollection<PrinterResponse>>;
