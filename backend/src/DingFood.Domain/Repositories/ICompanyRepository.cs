using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken cancellationToken = default);
    Task AddAsync(Company entity, CancellationToken cancellationToken = default);
}
