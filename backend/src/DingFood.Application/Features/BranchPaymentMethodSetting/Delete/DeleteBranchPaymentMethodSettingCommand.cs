using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.Delete
{
    public sealed record DeleteBranchPaymentMethodSettingCommand(
        long Id,
        long CompanyId) : ICommand;
}