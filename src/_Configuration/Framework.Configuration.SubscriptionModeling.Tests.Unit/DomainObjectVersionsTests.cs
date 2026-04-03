using FluentAssertions;

using Framework.Subscriptions.Domain;

using NUnit.Framework;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit;

[TestFixture]
public  class DomainObjectVersionsTests
{
    [Test]
    public void DomainObjectType_PreviousNotNull_PreviousType()
    {
        // Arrange
        var versions = new DomainObjectVersions<object>("1", null);

        // Act

        // Assert
        versions.DomainObjectType.Should().Be<string>();
    }

    [Test]
    public void DomainObjectType_CurrentNotNull_CurrentType()
    {
        // Arrange
        var versions = new DomainObjectVersions<object>(null, "1");

        // Act

        // Assert
        versions.DomainObjectType.Should().Be<string>();
    }

    [Test]
    public void DomainObjectType_BothNull_GenericParameterType()
    {
        // Arrange
        var versions = new DomainObjectVersions<string>("Test", null);

        // Act

        // Assert
        versions.DomainObjectType.Should().Be<string>();
    }

    [Test]
    public void DomainObjectType_PreviousAndCurrentHasDifferentTypes_Object()
    {
        // Arrange
        var versions = new DomainObjectVersions<object>("1", this.GetType());

        // Act

        // Assert
        versions.DomainObjectType.Should().Be<object>();
    }

    [Test]
    [TestCase(null, "B", DomainObjectChangeType.Create)]
    [TestCase("A", "B", DomainObjectChangeType.Update)]
    [TestCase("A", null, DomainObjectChangeType.Delete)]
    public void ChangeType_Get_CorrectComputedChangeType(
            object previous,
            object current,
            DomainObjectChangeType expectedChangeType)
    {
        // Arrange
        var versions = new DomainObjectVersions<object>(previous, current);

        // Act, Assert
        versions.ChangeType.Should().Be(expectedChangeType);
    }
}
