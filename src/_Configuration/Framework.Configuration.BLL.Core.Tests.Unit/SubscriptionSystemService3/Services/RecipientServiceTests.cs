using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Services;

[TestFixture]
public class RecipientServiceTests : TestFixtureBase
{
    private SubscriptionResolver subscriptionResolver;
    private RecipientsResolver<ITestBLLContext> recipientsResolver;
    private ConfigurationContextFacade configurationContextFacade;

    [SetUp]
    public void SetUp()
    {
        this.subscriptionResolver = this.Fixture.RegisterStub<SubscriptionResolver>();
        this.recipientsResolver = this.Fixture.RegisterStub<RecipientsResolver<ITestBLLContext>>();
        this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();
    }

    [Test]
    public void PublicSurface_NullArguments_ArgumentNullException()
    {
        // Arrange
        var assertion = new GuardClauseAssertion(this.Fixture);

        // Act

        // Assert
        assertion.Verify(typeof(RecipientService<ITestBLLContext>));
    }

    [Test]
    public void GetSubscriptionRecipientInfo_Call_SubscriptionRecipientInfo()
    {
        // Arrange
        var subscriptionCode = this.Fixture.Create<string>();
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();
        var subscription = this.Fixture.Create<Subscription>();
        var recipient = this.Fixture.Create<Recipient>();

        var bag = new RecipientsBag(
                                    new RecipientCollection(new[] { recipient }),
                                    new RecipientCollection(new[] { this.Fixture.Create<Recipient>() }),
                                    new RecipientCollection(new[] { this.Fixture.Create<Recipient>() }));

        var resolverResult = new RecipientsResolverResult(
                                                          bag,
                                                          this.Fixture.Create<DomainObjectVersions<object>>());

        this.subscriptionResolver
            .Resolve(subscriptionCode, versions)
            .Returns(subscription);

        this.recipientsResolver
            .Resolve(subscription, versions)
            .Returns(new[] { resolverResult });

        // Act
        var service = this.Fixture.Create<RecipientService<ITestBLLContext>>();
        var info = service.GetSubscriptionRecipientInfo(subscriptionCode, versions);

        // Assert
        info.Subscription.Should().Be(subscription);
        info.Recipients.Should().BeEquivalentTo(recipient.Email);
    }

    [Test]
    public void GetSubscriptionRecipientInfo_SubscriptionNotFound_Null()
    {
        // Arrange
        var subscriptionCode = this.Fixture.Create<string>();
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();

        this.subscriptionResolver
            .Resolve(subscriptionCode, versions)
            .Throws(new SubscriptionServicesException());

        var service = this.Fixture.Create<RecipientService<ITestBLLContext>>();

        // Act
        var info = service.GetSubscriptionRecipientInfo(subscriptionCode, versions);

        // Assert
        info.Should().BeNull();
    }
}
