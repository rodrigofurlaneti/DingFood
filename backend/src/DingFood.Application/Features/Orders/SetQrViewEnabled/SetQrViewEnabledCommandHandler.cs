using MediatR;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingFood.Application.Features.Orders.SetQrViewEnabled
{
    internal sealed class SetQrViewEnabledCommandHandler : IRequestHandler<SetQrViewEnabledCommand, Result>
    {
        private readonly IDiningTableRepository _diningTableRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SetQrViewEnabledCommandHandler(IDiningTableRepository diningTableRepository, IUnitOfWork unitOfWork)
        {
            _diningTableRepository = diningTableRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetQrViewEnabledCommand request, CancellationToken cancellationToken)
        {
            var tables = await _diningTableRepository.GetByBranchAsync(request.BranchId, cancellationToken);
            if (tables.Count == 0)
            {
                return Result.Success();
            }
            foreach (var table in tables)
            {
                table.SetQrViewEnabled(request.Enabled);
                _diningTableRepository.Update(table);
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success();
        }
    }
}
