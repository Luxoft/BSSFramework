using System;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Services
{
    [TestFixture]
    public sealed class TestingServiceTests : TestFixtureBase
    {
        private RevisionService<IdentityObject> revisionService;
        private ConditionLambdaProcessor<ITestBLLContext> conditionLambdaProcessor;
        private SubscriptionNotificationService<ITestBLLContext> notificationService;
        private ConfigurationContextFacade configurationContextFacade;

        [SetUp]
        public void SetUp()
        {
            var servicesFactory = this.Fixture.RegisterStub<SubscriptionServicesFactory<ITestBLLContext>>();
            this.revisionService = this.Fixture.RegisterStub<RevisionService<IdentityObject>>();
            this.conditionLambdaProcessor = this.Fixture.RegisterStub<ConditionLambdaProcessor<ITestBLLContext>>();
            this.notificationService = this.Fixture.RegisterStub<SubscriptionNotificationService<ITestBLLContext>>();
            this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();

            servicesFactory
                .CreateRevisionService(typeof(IdentityObject))
                .Returns(this.revisionService);

            servicesFactory
                .CreateLambdaProcessor<ConditionLambdaProcessor<ITestBLLContext>>()
                .Returns(this.conditionLambdaProcessor);

            servicesFactory
                .CreateNotificationService()
                .Returns(this.notificationService);
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture, new NullReferenceBehaviorExpectation());

            // Act

            // Assert
            assertion.Verify(typeof(TestingService<ITestBLLContext>));
        }

        [Test]
        public void TestSubscription_Call_NonEmptyTryResultCollection()
        {
            // Arrange
            var domainType = Substitute.ForPartsOf<DomainType>();
            domainType.FullTypeName.Returns(typeof(IdentityObject).FullName);

            var subscription = this.Fixture.Create<Subscription>();
            subscription.DomainType = domainType;

            var revision = this.Fixture.Create<long?>();
            var domainObjectId = this.Fixture.Create<Guid>();

            var versions = this.Fixture.Create<DomainObjectVersions<IdentityObject>>();

            this.configurationContextFacade
                .GetDomainObjectType(subscription.DomainType)
                .Returns(typeof(IdentityObject));

            this.revisionService
                .GetDomainObjectVersions(domainObjectId, revision)
                .Returns(versions);

            this.conditionLambdaProcessor
                .Invoke(subscription, versions)
                .Returns(true);

            this.notificationService
                .NotifyDomainObjectChanged(subscription, versions);

            // Act
            var service = this.Fixture.Create<TestingService<ITestBLLContext>>();
            var result = service.TestSubscription(subscription, domainObjectId, revision);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess().Should().BeTrue();
            result.GetValue().Should().Be(subscription);
        }

        [Test]
        public void TestSubscription_FalseSubscriptionCondition_FaultResult()
        {
            // Arrange
            var subscription = this.Fixture.Create<Subscription>();
            var revision = this.Fixture.Create<long?>();
            var domainObjectId = this.Fixture.Create<Guid>();

            this.configurationContextFacade
                .GetDomainObjectType(Arg.Any<DomainType>())
                .Returns(typeof(IdentityObject));

            this.conditionLambdaProcessor
                .Invoke(Arg.Any<Subscription>(), Arg.Any<DomainObjectVersions<IdentityObject>>())
                .Returns(false);

            // Act
            var service = this.Fixture.Create<TestingService<ITestBLLContext>>();
            var result = service.TestSubscription(subscription, domainObjectId, revision);

            // Assert
            result.IsFault().Should().BeTrue();
        }
    }
}
