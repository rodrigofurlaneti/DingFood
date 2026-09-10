using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Tables.GetByBranch;

public sealed record GetTablesByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<TableResponse>>;
