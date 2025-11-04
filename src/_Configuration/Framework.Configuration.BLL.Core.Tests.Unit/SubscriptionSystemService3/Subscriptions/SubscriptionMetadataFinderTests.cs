using FluentAssertions;

using Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions;

[TestFixture]
public sealed class SubscriptionMetadataFinderTests
{
    [Test]
    public void Find_Call_SubscriptionFound()
    {
        // Arrange
        var finder = new TestSubscriptionMetadataFinder();

        // Act
        var subscription = finder.Find().SingleOrDefault();

        // Assert
        subscription.Should().NotBeNull();
        subscription.Should().BeOfType<ObjectChangingSubscription>();
    }

    private class TestSubscriptionMetadataFinder() : SubscriptionMetadataFinder(
        new ServiceCollection().BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }),
        [new SubscriptionMetadataFinderAssemblyInfo(typeof(TestSubscriptionMetadataFinder).Assembly)]);
}
