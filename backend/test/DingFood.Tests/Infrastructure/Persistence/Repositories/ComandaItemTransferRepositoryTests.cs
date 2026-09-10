using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Infrastructure.Persistence.Repositories;
using Xunit;

namespace DingFood.Tests.Infrastructure.Persistence.Repositories
{
    public sealed class ComandaItemTransferRepositoryTests : RepositoryTestBase
    {
        private readonly ComandaItemTransferRepository _repository;

        public ComandaItemTransferRepositoryTests()
        {
            _repository = new ComandaItemTransferRepository(Context);
        }

        [Fact]
        public async Task AddAsync_ValidTransfer_PersistsToDatabase()
        {
            var transfer = ComandaItemTransfer.Create(
                customerOrderId: 1, customerOrderItemId: 1, sourceComandaId: 1, targetComandaId: 2, employeeId: 1).Value;

            await _repository.AddAsync(transfer);
            await Context.SaveChangesAsync();

            var persisted = await Context.Set<ComandaItemTransfer>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SourceComandaId == 1 && x.TargetComandaId == 2);

            persisted.Should().NotBeNull();
            persisted!.Id.Should().BeGreaterThan(0);
        }
    }
}
