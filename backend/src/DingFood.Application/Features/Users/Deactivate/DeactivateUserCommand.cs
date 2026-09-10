using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Users.Deactivate;

public sealed record DeactivateUserCommand(long AppUserId) : ICommand;
