using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3;

public sealed class SubscriptionSystemServiceTests : TestFixtureBase
{
    private SubscriptionNotificationService<ITestBLLContext> notificationService;
    private RecipientService<ITestBLLContext> recipientService;

    [SetUp]
    public void SetUp()
    {
        this.notificationService = this.Fixture.RegisterStub<SubscriptionNotificationService<ITestBLLContext>>();
        this.recipientService = this.Fixture.RegisterStub<RecipientService<ITestBLLContext>>();

        var servicesFactory = this.Fixture.RegisterStub<SubscriptionServicesFactory<ITestBLLContext>>();
        servicesFactory.CreateNotificationService().Returns(this.notificationService);
        servicesFactory.CreateRecipientService().Returns(this.recipientService);
    }

    [Test]
    public void PublicSurface_NullArguments_ArgumentNullException()
    {
        // Arrange
        var assertion = new GuardClauseAssertion(this.Fixture);
        var targetType = typeof(RevisionSubscriptionSystemService<ITestBLLContext, IdentityObject >);

        // Act

        // Assert
        assertion.Verify(targetType.GetConstructors());
        assertion.Verify(
                         targetType.GetMethods()
                                   .Where(mi => mi.Name != "GetRecipientsUntyped" && mi.Name != "ProcessChangedObjectUntyped"));
    }

    [Test]
    public void GetRecipientsUntyped_NullType_ArgumentNullException()
    {
        // Arrange
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();
        Action call = () => sut.GetRecipientsUntyped(null, string.Empty, string.Empty, string.Empty);

        // Act

        // Assert
        call.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetRecipientsUntyped_NullSubscriptionCode_ArgumentNullException()
    {
        // Arrange
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();
        Action call = () => sut.GetRecipientsUntyped(typeof(string), string.Empty, string.Empty, null);

        // Act

        // Assert
        call.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetRecipientsUntyped_NullPrev_NoArgumentNullException()
    {
        // Arrange
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();
        Action call = () => sut.GetRecipientsUntyped(typeof(string), null, string.Empty, string.Empty);

        // Act

        // Assert
        call.Should().NotThrow<ArgumentNullException>();
    }

    [Test]
    public void GetRecipientsUntyped_NullNext_NoArgumentNullException()
    {
        // Arrange
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();
        Action call = () => sut.GetRecipientsUntyped(typeof(string), string.Empty, null, string.Empty);

        // Act

        // Assert
        call.Should().NotThrow<ArgumentNullException>();
    }

    [Test]
    public void ProcessChangedObjectUntyped_NullType_ArgumentNullException()
    {
        // Arrange
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();
        Action call = () => sut.ProcessChangedObjectUntyped(string.Empty, string.Empty, null);

        // Act

        // Assert
        call.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ProcessChangedObjectUntyped_NullPrev_NoArgumentNullException()
    {
        // Arrange
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();
        Action call = () => sut.ProcessChangedObjectUntyped(null, string.Empty, typeof(string));

        // Act

        // Assert
        call.Should().NotThrow<ArgumentNullException>();
    }

    [Test]
    public void ProcessChangedObjectUntyped_NullNext_NoArgumentNullException()
    {
        // Arrange
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();
        Action call = () => sut.ProcessChangedObjectUntyped(string.Empty, null, typeof(string));

        // Act

        // Assert
        call.Should().NotThrow<ArgumentNullException>();
    }

    [Test]
    public void ProcessChangedObjectUntyped_Call_NonEmptyTryResultCollection()
    {
        // Arrange
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();
        var expectedResult = Substitute.For<IList<ITryResult<Subscription>>>();

        this.notificationService
            .NotifyDomainObjectChanged(versions)
            .Returns(expectedResult);

        // Act
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();

        var actualResult = sut.ProcessChangedObjectUntyped(
                                                           versions.Previous,
                                                           versions.Current,
                                                           versions.DomainObjectType);

        // Assert
        actualResult.Should().BeSameAs(expectedResult);
    }

    [Test]
    public void GetRecipientsUntyped_Call_SubscriptionRecipientInfo()
    {
        // Arrange
        var subscriptionCode = this.Fixture.Create<string>();
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();
        var expectedResult = this.Fixture.Create<SubscriptionRecipientInfo>();

        this.recipientService
            .GetSubscriptionRecipientInfo(subscriptionCode, versions)
            .Returns(expectedResult);

        // Act
        var sut = this.Fixture.Create<SubscriptionSystemService<ITestBLLContext>>();

        var actualResult = sut.GetRecipientsUntyped(
                                                    versions.DomainObjectType,
                                                    versions.Previous,
                                                    versions.Current,
                                                    subscriptionCode);

        // Assert
        actualResult.Should().BeSameAs(expectedResult);
    }
}
