using FluentAssertions;
using FluentValidation;
using NetArchTest.Rules;
using DingFood.Domain.Primitives;
using Xunit;

namespace DingFood.ArchTests;

public sealed class ArchitectureTests
{
    private const string ApplicationNamespace = "DingFood.Application";
    private const string InfrastructureNamespace = "DingFood.Infrastructure";
    private const string ApiNamespace = "DingFood.API";

    private static readonly System.Reflection.Assembly DomainAssembly = typeof(Entity).Assembly;
    private static readonly System.Reflection.Assembly ApplicationAssembly = typeof(DingFood.Application.DependencyInjection).Assembly;
    private static readonly System.Reflection.Assembly InfrastructureAssembly = typeof(DingFood.Infrastructure.DependencyInjection).Assembly;

    [Fact]
    public void Domain_ShouldNotDependOn_OuterLayersOrFrameworks()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace, ApiNamespace,
                "MediatR", "Microsoft.EntityFrameworkCore", "FluentValidation")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNotDependOn_InfrastructureOrApi()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, ApiNamespace, "Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOn_Api()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlers_ShouldBe_InternalSealed()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That().HaveNameEndingWith("CommandHandler")
            .Should().BeClasses().And().BeSealed().And().NotBePublic()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_ShouldBe_InternalSealed()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That().HaveNameEndingWith("QueryHandler")
            .Should().BeClasses().And().BeSealed().And().NotBePublic()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Repositories_ShouldBe_InternalSealed_InPersistence()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
            .That().HaveNameEndingWith("Repository")
            .Should().BeSealed().And().NotBePublic()
            .And().ResideInNamespace("DingFood.Infrastructure.Persistence.Repositories")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void EfConfigurations_ShouldBe_InternalSealed()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
            .That().HaveNameEndingWith("Configuration")
            .And().ResideInNamespace("DingFood.Infrastructure.Persistence.Configurations")
            .Should().BeSealed().And().NotBePublic()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Responses_ShouldBe_Sealed()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That().HaveNameEndingWith("Response")
            .Should().BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validators_ShouldInherit_AbstractValidator()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That().HaveNameEndingWith("Validator")
            .Should().Inherit(typeof(AbstractValidator<>))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandsAndQueries_ShouldResideIn_Features()
    {
        var commands = Types.InAssembly(ApplicationAssembly)
            .That().AreClasses().And().HaveNameEndingWith("Command")
            .Should().ResideInNamespace("DingFood.Application.Features")
            .GetResult();

        var queries = Types.InAssembly(ApplicationAssembly)
            .That().AreClasses().And().HaveNameEndingWith("Query")
            .Should().ResideInNamespace("DingFood.Application.Features")
            .GetResult();

        commands.IsSuccessful.Should().BeTrue();
        queries.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldBe_Sealed()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That().ResideInNamespace("DingFood.Domain.Entities")
            .And().AreClasses()
            .Should().BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Aggregates_ShouldInherit_AggregateRoot()
    {
        var aggregates = new[]
        {
            typeof(DingFood.Domain.Entities.Company), typeof(DingFood.Domain.Entities.Branch),
            typeof(DingFood.Domain.Entities.AppUser), typeof(DingFood.Domain.Entities.CustomerOrder),
            typeof(DingFood.Domain.Entities.Sale), typeof(DingFood.Domain.Entities.CashSession),
            typeof(DingFood.Domain.Entities.StockItem), typeof(DingFood.Domain.Entities.Purchase)
        };

        foreach (var type in aggregates)
            type.Should().BeAssignableTo<AggregateRoot>();
    }

    [Fact]
    public void ChildEntities_ShouldInherit_Entity_NotAggregateRoot()
    {
        var children = new[]
        {
            typeof(DingFood.Domain.Entities.OrderItem), typeof(DingFood.Domain.Entities.SalePayment),
            typeof(DingFood.Domain.Entities.CashMovement), typeof(DingFood.Domain.Entities.StockMovement),
            typeof(DingFood.Domain.Entities.PurchaseItem)
        };

        foreach (var type in children)
        {
            type.Should().BeAssignableTo<Entity>();
            type.Should().NotBeAssignableTo<AggregateRoot>();
        }
    }

    [Fact]
    public void RepositoryInterfaces_ShouldResideIn_DomainRepositories()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That().AreInterfaces().And().HaveNameEndingWith("Repository")
            .Should().ResideInNamespace("DingFood.Domain.Repositories")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
