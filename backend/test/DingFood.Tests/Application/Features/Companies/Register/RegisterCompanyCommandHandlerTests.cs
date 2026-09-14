using FluentAssertions;
using NSubstitute;
using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Features.Companies.Register;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using Xunit;

namespace DingFood.Tests.Application.Features.Companies.Register;

public sealed class RegisterCompanyCommandHandlerTests
{
    private readonly ICompanyRepository _companyRepository = Substitute.For<ICompanyRepository>();
    private readonly IBranchRepository _branchRepository = Substitute.For<IBranchRepository>();
    private readonly IBrandRepository _brandRepository = Substitute.For<IBrandRepository>();
    private readonly IRoleRepository _roleRepository = Substitute.For<IRoleRepository>();
    private readonly IAppUserRepository _userRepository = Substitute.For<IAppUserRepository>();
    private readonly IUserRoleRepository _userRoleRepository = Substitute.For<IUserRoleRepository>();
    private readonly IDiningTableRepository _diningTableRepository = Substitute.For<IDiningTableRepository>();
    private readonly IComandaRepository _comandaRepository = Substitute.For<IComandaRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IJobTitleRepository _jobTitleRepository = Substitute.For<IJobTitleRepository>();
    private readonly IEmployeeRepository _employeeRepository = Substitute.For<IEmployeeRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IBusinessGroupRepository _businessGroupRepository = Substitute.For<IBusinessGroupRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly RegisterCompanyCommandHandler _handler;

    public RegisterCompanyCommandHandlerTests()
    {
        _handler = new RegisterCompanyCommandHandler(
            _companyRepository,
            _branchRepository,
            _brandRepository,
            _roleRepository,
            _userRepository,
            _userRoleRepository,
            _diningTableRepository,
            _comandaRepository,
            _categoryRepository,
            _jobTitleRepository,
            _employeeRepository,
            _passwordHasher,
            _logRepository,
            _businessGroupRepository,
            _unitOfWork);

        // Por padrão nenhuma unicidade conflita e o hash é determinístico — cada teste
        // sobrescreve apenas o que precisa testar.
        _companyRepository.ExistsByCnpjAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _userRepository.ExistsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _employeeRepository.ExistsByCpfAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashed-password");
    }

    private static RegisterCompanyCommand CreateCommand() => new(
        LegalName: "Bar do Rodrigo LTDA",
        TradeName: "Bar do Rodrigo",
        Cnpj: "12345678000199",
        CompanyEmail: "contato@bardorodrigo.com",
        CompanyPhone: "11999990000",
        BranchName: "Matriz",
        BranchCnpj: "12345678000199",
        AddressStreet: "Rua das Flores",
        AddressNumber: "100",
        AddressDistrict: "Centro",
        AddressCity: "São Paulo",
        AddressState: "SP",
        AddressZipCode: "01000000",
        AdminName: "Rodrigo Furlaneti",
        AdminCpf: "12345678900",
        AdminUserName: "rodrigo.admin",
        AdminEmail: "rodrigo@bardorodrigo.com",
        AdminPassword: "Senha@123");

    [Fact]
    public async Task Handle_CompanyCnpjAlreadyExists_ShouldReturnFailureWithoutPersistingAnything()
    {
        var command = CreateCommand();
        _companyRepository.ExistsByCnpjAsync(command.Cnpj, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Company.AlreadyExists");

        await _companyRepository.DidNotReceive().AddAsync(Arg.Any<Company>(), Arg.Any<CancellationToken>());
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<AppUser>(), Arg.Any<CancellationToken>());
        // Nenhuma checagem de unicidade posterior deveria sequer rodar.
        await _userRepository.DidNotReceive().ExistsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AdminUserNameOrEmailAlreadyExists_ShouldReturnFailureWithoutPersistingAnything()
    {
        var command = CreateCommand();
        _userRepository.ExistsAsync(command.AdminUserName, command.AdminEmail, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("AppUser.AlreadyExists");

        await _companyRepository.DidNotReceive().AddAsync(Arg.Any<Company>(), Arg.Any<CancellationToken>());
        await _employeeRepository.DidNotReceive().ExistsByCpfAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AdminCpfAlreadyExists_ShouldReturnFailureWithoutPersistingAnything()
    {
        var command = CreateCommand();
        _employeeRepository.ExistsByCpfAsync(command.AdminCpf, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Employee.AlreadyExists");

        await _companyRepository.DidNotReceive().AddAsync(Arg.Any<Company>(), Arg.Any<CancellationToken>());
        await _branchRepository.DidNotReceive().AddAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}
