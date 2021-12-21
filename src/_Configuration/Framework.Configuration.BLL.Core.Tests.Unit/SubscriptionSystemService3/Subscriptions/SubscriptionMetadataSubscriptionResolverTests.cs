using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using FluentAssertions;
using Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions
{
    [TestFixture]
    public sealed class SubscriptionMetadataSubscriptionResolverTests : TestFixtureBase
    {
        private SubscriptionMetadataStore metadataStore;
        private SubscriptionMetadataMapper metadataMapper;
        private SubscriptionResolver wrappedResolver;
        private ConfigurationContextFacade configurationContextFacade;

        [SetUp]
        public void SetUp()
        {
            this.metadataStore = this.Fixture.RegisterStub<SubscriptionMetadataStore>();
            this.metadataMapper = this.Fixture.RegisterStub<SubscriptionMetadataMapper>();
            this.wrappedResolver = this.Fixture.RegisterStub<SubscriptionResolver>();
            this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();

            this.Fixture.Customize<SubscriptionMetadataSubscriptionResolver>(
                c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(SubscriptionMetadataSubscriptionResolver));
        }

        [Test]
        public void Resolve_DomainObjectVersions_SubscriptionsCollection()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<object>>();
            var metadata = new ObjectChangingSubscription();

            var subscriptionByDb = this.Fixture.Create<Subscription>();

            var subscriptionByMetadata = this.Fixture
                .Build<Subscription>().With(s => s.Code, metadata.Code)
                .Create();

            var expectedCollection = new List<Subscription> { subscriptionByDb, subscriptionByMetadata };

            this.metadataStore
                .Get(typeof(object))
                .Returns(new[] { metadata });

            this.metadataMapper
                .Map(metadata)
                .Returns(subscriptionByMetadata);

            this.wrappedResolver
                .Resolve(versions)
                .Returns(new[] { subscriptionByDb });

            this.configurationContextFacade
                .GetActiveCodeFirstSubscriptionCodes()
                .Returns(new[] { metadata.Code });

            var resolver = this.Fixture.Create<SubscriptionMetadataSubscriptionResolver>();

            // Act
            var actualCollection = resolver.Resolve(versions);

            // Assert
            actualCollection.Should().BeEquivalentTo(expectedCollection);
        }

        [Test]
        public void Resolve_NoDbSubscriptions_SubscriptionCode_Subscription()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<object>>();
            var metadata = new ObjectChangingSubscription();

            var expectedSubscription = this.Fixture
                .Build<Subscription>().With(s => s.Code, metadata.Code)
                .Create();

            this.metadataStore
                .Get(typeof(object))
                .Returns(new[] { metadata });

            this.metadataMapper
                .Map(metadata)
                .Returns(expectedSubscription);

            this.wrappedResolver
                .Resolve(versions)
                .Returns(Enumerable.Empty<Subscription>());

            this.configurationContextFacade
                .GetActiveCodeFirstSubscriptionCodes()
                .Returns(new[] { metadata.Code });

            var resolver = this.Fixture.Create<SubscriptionMetadataSubscriptionResolver>();

            // Act
            var actualSubscription = resolver.Resolve(metadata.Code, versions);

            // Assert
            actualSubscription.Should().BeSameAs(expectedSubscription);
        }

        [Test]
        public void Resolve_NoMetadataSubscriptions_SubscriptionCode_Subscription()
        {
            // Arrange
            const string Code = "Code 1";
            var versions = this.Fixture.Create<DomainObjectVersions<object>>();

            var expectedSubscription = this.Fixture.Create<Subscription>();
            expectedSubscription.Code = Code;

            this.metadataStore
                .Get(typeof(object))
                .Returns(Enumerable.Empty<ISubscriptionMetadata>());

            this.wrappedResolver
                .Resolve(versions)
                .Returns(new[] { expectedSubscription });

            var resolver = this.Fixture.Create<SubscriptionMetadataSubscriptionResolver>();

            // Act
            var actualSubscription = resolver.Resolve(Code, versions);

            // Assert
            actualSubscription.Should().BeSameAs(expectedSubscription);
        }

        [Test]
        public void IsActiveSubscriptionForTypeExists_NoCodeFirstSubscriptions_CallWrappedResover_True()
        {
            // Arrange
            var type = this.GetType();

            this.metadataStore
                .Get(type)
                .Returns(Enumerable.Empty<ISubscriptionMetadata>());

            this.wrappedResolver
                .IsActiveSubscriptionForTypeExists(type)
                .Returns(true);

            // Act
            var resolver = this.Fixture.Create<SubscriptionMetadataSubscriptionResolver>();
            var result = resolver.IsActiveSubscriptionForTypeExists(type);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsActiveSubscriptionForTypeExists_NoDbFirstSubscriptions_CallMetadataStore_True()
        {
            // Arrange
            var subscriptionMetadata = new ObjectChangingSubscription();

            var type = this.GetType();

            this.wrappedResolver
                .IsActiveSubscriptionForTypeExists(type)
                .Returns(false);

            this.metadataStore
                .Get(type)
                .Returns(new[] { subscriptionMetadata });

            this.configurationContextFacade
                .GetActiveCodeFirstSubscriptionCodes()
                .Returns(new[] { subscriptionMetadata.Code });

            this.metadataMapper
                .Map(subscriptionMetadata)
                .Returns(new Subscription());

            // Act
            var resolver = this.Fixture.Create<SubscriptionMetadataSubscriptionResolver>();
            var result = resolver.IsActiveSubscriptionForTypeExists(type);

            // Assert
            result.Should().BeTrue();
        }
    }
}
