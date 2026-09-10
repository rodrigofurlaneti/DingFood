using FluentValidation;
using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.CustomerAppUser.Remove
{
    public sealed record RemoveCustomerAppUserCommand(long Id) : ICommand;
}
