using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Printing.GetSettings;

public sealed record GetPrintSettingsQuery(long BranchId) : IQuery<PrintSettingsResponse>;
