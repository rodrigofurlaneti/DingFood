using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Printing.DeactivatePrinter;

public sealed record DeactivatePrinterCommand(long PrinterId) : ICommand;
