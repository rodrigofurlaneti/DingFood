using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Employees.CreateJobTitle;

public sealed record CreateJobTitleCommand(long CompanyId, string Name) : ICommand<long>;
