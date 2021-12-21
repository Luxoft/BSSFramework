using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions
{
    [TestFixture]
    public sealed class DomainObjectSubscriptionsResolverTests : TestFixtureBase
    {
        private ConfigurationContextFacade configurationContextFacade;

        [SetUp]
        public void SetUp()
        {
            this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(DomainObjectSubscriptionsResolver));
        }

        [Test]
        public void Resolve_NonNullDomainObjectType_CollectionOfSubscriptions()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var subscriptions = this.Fixture.CreateMany<Subscription>().AsQueryable();

            this.configurationContextFacade
                .GetSubscriptions(versions.DomainObjectType)
                .Returns(subscriptions);

            // Act
            var resolver = this.Fixture.Create<DomainObjectSubscriptionsResolver>();
            var callResult = resolver.Resolve(versions);

            // Assert
            callResult.Should().BeEquivalentTo(subscriptions);
        }

        [Test]
        public void ResolveByCode_SubscriptionFound_Subscription()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var subscriptions = this.Fixture.CreateMany<Subscription>().AsQueryable();

            var expectedSubscription = subscriptions.Last();

            this.configurationContextFacade
                .GetSubscriptions(versions.DomainObjectType)
                .Returns(subscriptions);

            // Act
            var resolver = this.Fixture.Create<DomainObjectSubscriptionsResolver>();
            var actualSubscription = resolver.Resolve(expectedSubscription.Code, versions);

            // Assert
            actualSubscription.Should().Be(expectedSubscription);
        }

        [Test]
        public void ResolveByCode_SubscriptionNotFound_Exception()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var subscriptions = new List<Subscription>().AsQueryable();

            this.configurationContextFacade
                .GetSubscriptions(versions.DomainObjectType)
                .Returns(subscriptions);

            // Act
            var resolver = this.Fixture.Create<DomainObjectSubscriptionsResolver>();
            Action call = () =>  resolver.Resolve("some code", versions);

            // Assert
            call.Should().Throw<SubscriptionServicesException>();
        }

        [Test]
        public void IsActiveSubscriptionForTypeExists_Call_True()
        {
            // Arrange
            var type = this.GetType();

            this.configurationContextFacade
                .IsActiveSubscriptionsExists(type)
                .Returns(true);

            // Act
            var resolver = this.Fixture.Create<DomainObjectSubscriptionsResolver>();
            var result = resolver.IsActiveSubscriptionForTypeExists(type);

            // Assert
            result.Should().BeTrue();
        }
    }
}
