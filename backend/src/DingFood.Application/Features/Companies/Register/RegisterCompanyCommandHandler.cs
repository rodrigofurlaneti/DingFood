using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
using MediatR;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;

namespace DingFood.Application.Features.Companies.Register;

internal sealed class RegisterCompanyCommandHandler : BaseCommandHandler<RegisterCompanyCommand, RegisterCompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAppUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IDiningTableRepository _diningTableRepository;
    private readonly IComandaRepository _comandaRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IBusinessGroupRepository _groupsRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    private static readonly string[] DefaultCategoryNames =
    [
        "Bebidas",
        "Petiscos",
        "Pratos Principais",
        "Drinks",
        "Sobremesas"
    ];

    public RegisterCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IBranchRepository branchRepository,
        IBrandRepository brandRepository,
        IRoleRepository roleRepository,
        IAppUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IDiningTableRepository diningTableRepository,
        IComandaRepository comandaRepository,
        ICategoryRepository categoryRepository,
        IJobTitleRepository jobTitleRepository,
        IEmployeeRepository employeeRepository,
        IPasswordHasher passwordHasher,
        ILogTrackerRepository logRepository,
        IBusinessGroupRepository groupsRepository,
    IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _companyRepository = companyRepository;
        _branchRepository = branchRepository;
        _brandRepository = brandRepository;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _diningTableRepository = diningTableRepository;
        _comandaRepository = comandaRepository;
        _categoryRepository = categoryRepository;
        _jobTitleRepository = jobTitleRepository;
        _employeeRepository = employeeRepository;
        _passwordHasher = passwordHasher;
        _groupsRepository = groupsRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<RegisterCompanyResponse>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(RegisterCompanyCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var uniquenessResult = await ValidateUniquenessAsync(request, cancellationToken);
                if (uniquenessResult.IsFailure)
                    return Result.Failure<RegisterCompanyResponse>(uniquenessResult.Error);

                var structureResult = await SetupCompanyStructureAsync(request, cancellationToken);
                if (structureResult.IsFailure)
                    return Result.Failure<RegisterCompanyResponse>(structureResult.Error);
                var (company, branch) = structureResult.Value;

                var adminResult = await SetupAdminAccountAsync(request, company.Id, branch.Id, cancellationToken);
                if (adminResult.IsFailure)
                    return Result.Failure<RegisterCompanyResponse>(adminResult.Error);
                var user = adminResult.Value;

                userIdBox.Value = user.Id;

                return Result.Success(new RegisterCompanyResponse(company.Id, branch.Id, user.Id));
            });
    }

    private async Task<Result<(Company Company, Branch Branch)>> SetupCompanyStructureAsync(
        RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyResult = await CreateCompanyAsync(request, cancellationToken);
        if (companyResult.IsFailure)
            return Result.Failure<(Company, Branch)>(companyResult.Error);
        var company = companyResult.Value;

        await CreateDefaultCategoriesAsync(company.Id, cancellationToken);

        var branchResult = await CreateBranchAsync(request, company.Id, cancellationToken);
        if (branchResult.IsFailure)
            return Result.Failure<(Company, Branch)>(branchResult.Error);
        var branch = branchResult.Value;

        return Result.Success((company, branch));
    }

    private async Task<Result<AppUser>> SetupAdminAccountAsync(
    RegisterCompanyCommand request,
    long companyId,
    long branchId,
    CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        long? brandId = branch.BrandId;
        var jobTitleResult = await CreateAdminJobTitleAsync(companyId, brandId ?? 0, cancellationToken);
        if (jobTitleResult.IsFailure)
            return Result.Failure<AppUser>(jobTitleResult.Error);

        var jobTitle = jobTitleResult.Value;

        var employeeResult = await CreateAdminEmployeeAsync(request, branchId, jobTitle.Id, cancellationToken);
        if (employeeResult.IsFailure)
            return Result.Failure<AppUser>(employeeResult.Error);
        var employee = employeeResult.Value;

        var roleResult = await CreateAdminRoleAsync(companyId, cancellationToken);
        if (roleResult.IsFailure)
            return Result.Failure<AppUser>(roleResult.Error);
        var role = roleResult.Value;

        var userResult = await CreateAdminUserAsync(request, companyId, employee.Id, cancellationToken);
        if (userResult.IsFailure)
            return Result.Failure<AppUser>(userResult.Error);
        var user = userResult.Value;

        var linkResult = await LinkUserToRoleAsync(companyId, user.Id, role.Id, cancellationToken);
        if (linkResult.IsFailure)
            return Result.Failure<AppUser>(linkResult.Error);

        return Result.Success(user);
    }

    private async Task<Result> ValidateUniquenessAsync(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        if (await _companyRepository.ExistsByCnpjAsync(request.Cnpj, cancellationToken))
            return Result.Failure(
                new Error("Company.AlreadyExists", "A company with this CNPJ is already registered."));

        if (await _userRepository.ExistsAsync(request.AdminUserName, request.AdminEmail, cancellationToken))
            return Result.Failure(
                new Error("AppUser.AlreadyExists", "User name or e-mail already in use."));

        if (await _employeeRepository.ExistsByCpfAsync(request.AdminCpf, cancellationToken))
            return Result.Failure(
                new Error("Employee.AlreadyExists", "A employee with this CPF is already registered."));

        return Result.Success();
    }

    private async Task CreateDefaultCategoriesAsync(long companyId, CancellationToken cancellationToken)
    {
        var displayOrder = 0;
        foreach (var categoryName in DefaultCategoryNames)
        {
            var categoryResult = Category.Create(companyId, categoryName, displayOrder++);
            if (categoryResult.IsSuccess)
                await _categoryRepository.AddAsync(categoryResult.Value, cancellationToken);
        }
    }

    private async Task<Result<Branch>> CreateBranchAsync(RegisterCompanyCommand request, long companyId, CancellationToken cancellationToken)
    {
        var nameBranch = "Filial " + request.BranchName;
        var branchResult = Branch.Create(
            companyId, nameBranch, request.Cnpj, request.CompanyPhone,
            request.AddressStreet, request.AddressNumber, request.AddressDistrict,
            request.AddressCity, request.AddressState, request.AddressZipCode);
        if (branchResult.IsFailure)
            return branchResult;

        await _branchRepository.AddAsync(branchResult.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken); 

        return branchResult;
    }

    private async Task<Result<JobTitle>> CreateAdminJobTitleAsync(long companyId, long brandId, CancellationToken cancellationToken)
    {
        var jobTitleResult = JobTitle.Create(companyId, brandId, "Administrador");
        if (jobTitleResult.IsFailure)
            return jobTitleResult;

        await _jobTitleRepository.AddAsync(jobTitleResult.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return jobTitleResult;
    }

    private async Task<Result<Employee>> CreateAdminEmployeeAsync(
        RegisterCompanyCommand request, long branchId, long jobTitleId, CancellationToken cancellationToken)
    {
        var employeeResult = Employee.Create(
            branchId, jobTitleId, request.AdminName, request.AdminCpf,
            request.AdminEmail, request.CompanyPhone, DateTime.Now, null, null);
        if (employeeResult.IsFailure)
            return employeeResult;

        await _employeeRepository.AddAsync(employeeResult.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return employeeResult;
    }

    private async Task<Result<Role>> CreateAdminRoleAsync(long companyId, CancellationToken cancellationToken)
    {
        var roleResult = Role.Create(companyId, "Administrador", "Acesso total — criado no onboarding.");
        if (roleResult.IsFailure)
            return roleResult;

        await _roleRepository.AddAsync(roleResult.Value, cancellationToken);

        return roleResult;
    }

    private async Task<Result<AppUser>> CreateAdminUserAsync(
        RegisterCompanyCommand request, long companyId, long employeeId, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordHasher.Hash(request.AdminPassword);
        var userResult = AppUser.Create(companyId, employeeId, request.AdminUserName, request.AdminEmail, passwordHash);
        if (userResult.IsFailure)
            return userResult;

        await _userRepository.AddAsync(userResult.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken); // precisa dos Ids de Role/AppUser para o vínculo

        return userResult;
    }

    private async Task<Result> LinkUserToRoleAsync(long companyId, long userId, long roleId, CancellationToken cancellationToken)
    {
        var linkResult = UserRole.Create(companyId, userId, roleId);
        if (linkResult.IsFailure)
            return Result.Failure(linkResult.Error);

        await _userRoleRepository.AddAsync(linkResult.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result<Company>> CreateCompanyAsync(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        string primeiroNome = request.LegalName.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? request.LegalName;
        string nameGrupo = "Grupo " + primeiroNome;

        var groupResult = BusinessGroup.Create(nameGrupo);
        if (groupResult.IsFailure)
            return Result.Failure<Company>(groupResult.Error); // Sempre valide o Result do grupo também

        var companyResult = Company.Create(
            request.LegalName, request.TradeName, request.Cnpj, request.CompanyEmail, request.CompanyPhone);
        if (companyResult.IsFailure)
            return companyResult;

        var association = companyResult.Value.AssignToGroup(groupResult.Value);
        if (association.IsFailure)
            return Result.Failure<Company>(association.Error);

        await _companyRepository.AddAsync(companyResult.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return companyResult;
    }
}