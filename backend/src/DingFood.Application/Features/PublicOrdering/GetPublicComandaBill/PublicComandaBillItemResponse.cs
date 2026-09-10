using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingFood.Application.Features.PublicOrdering.GetPublicComandaBill
{
    public sealed record PublicComandaBillItemResponse(
        long ItemId,
        string ProductName,
        decimal Quantity,
        decimal UnitPrice,
        decimal TotalPrice,
        long StatusId,
        DateTime RequestedAt,
        string? Notes
    )
    {
        public IReadOnlyCollection<PublicItemCustomizationResponse> OptionalExtras { get; init; } = [];
        public IReadOnlyCollection<PublicItemCustomizationResponse> Boosts { get; init; } = [];
    }
}
