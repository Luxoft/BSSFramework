using FluentAssertions;

using Framework.AutomationCore.Unit.Queryable;

using NHibernate.Linq;

using Xunit;

namespace SampleSystem.UnitTests;

public class TestQueryableTests
{
    [Fact]
    public async Task NonGenericMethod_NotThrowException()
    {
        // Arrange
        var domainObject = new DomainObject { Name = Statuses.Value.GetName() };
        var queryable = new TestQueryable<DomainObject>(domainObject).Where(x => x.Name == Statuses.Value.GetName());

        // Act
        var result = await queryable.ToListAsync();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GenericMethod_NotThrowException()
    {
        // Arrange
        var domainObject = new DomainObject { Name = Statuses.Value.GetName() };
        var queryable = new TestQueryable<DomainObject>(domainObject)
                        .Where(x => x.Name == Statuses.Value.GetName())
                        .Fetch(x => x.Parent);

        // Act
        var result = await queryable.ToListAsync();

        // Assert
        result.Should().NotBeNull();
    }

    private class DomainObject
    {
        public string Name { get; set; }

        public DomainObject Parent { get; }
    }

    private enum Statuses
    {
        Value
    }
}

internal static class EnumExtensions
{
    public static string GetName(this Enum @enum) => @enum.ToString();
}
