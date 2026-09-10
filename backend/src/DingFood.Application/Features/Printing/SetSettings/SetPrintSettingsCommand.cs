using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Printing.SetSettings;

public sealed record SetPrintSettingsCommand(
    long BranchId,
    bool PrintOrdersEnabled,
    bool PrintBillsEnabled) : ICommand;
