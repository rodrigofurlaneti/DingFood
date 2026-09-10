using MediatR;
using DingFood.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingFood.Application.Features.Orders.GetQrViewSetting
{
    public sealed record GetQrViewSettingQuery(long BranchId) : IRequest<Result<QrViewSettingResponse>>;
}
