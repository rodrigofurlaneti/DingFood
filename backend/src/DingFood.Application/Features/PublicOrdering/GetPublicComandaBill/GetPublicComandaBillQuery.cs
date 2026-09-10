using MediatR;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingFood.Application.Features.PublicOrdering.GetPublicComandaBill
{
    public sealed record GetPublicComandaBillQuery(Guid TableToken, string ComandaCode) : IQuery<PublicComandaBillResponse>;
}
