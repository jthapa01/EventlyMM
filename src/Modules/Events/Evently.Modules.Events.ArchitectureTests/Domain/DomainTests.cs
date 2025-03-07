using System.Reflection;
using Evently.Common.Domain;
using Evently.Modules.Events.ArchitectureTests.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace Evently.Modules.Events.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Or()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void DomainEvent_ShouldHave_DomainEvent_As_Suffix()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Or()
            .Inherit(typeof(DomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void Entities_ShouldHave_Private_Parameterless_Constructor()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = (from entityType in entityTypes 
            let constructorsInfo = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance) 
            where !constructorsInfo.Any(c => c.IsPrivate && c.GetParameters().Length == 0) select entityType).ToList();

        failingTypes.Should().BeEmpty();
    }
    
    [Fact]
    public void Entities_ShouldOnlyHave_Private_Constructors()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();
        
        var failingTypes = ( from entityType in entityTypes 
            let constructorsInfo = entityType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                where constructorsInfo.Any() select entityType).ToList();
        
        failingTypes.Should().BeEmpty();
    }
}
