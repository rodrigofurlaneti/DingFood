using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.Update
{
    public sealed record UpdateBranchPaymentMethodSettingCommand(
        long Id,
        long CompanyId,
        bool? EnablePix = null,
        bool? EnableBoleto = null,
        bool? EnableCreditCard = null,
        bool? EnableDebitCard = null,
        bool? EnableCashMachine = null,
        bool? IsActive = null) : ICommand;
}