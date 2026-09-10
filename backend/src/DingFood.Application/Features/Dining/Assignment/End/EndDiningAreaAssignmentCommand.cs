using System;
using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Assignment.End
{
    public sealed record EndDiningAreaAssignmentCommand(
        long Id,
        DateTime EndAt) : ICommand;
}
