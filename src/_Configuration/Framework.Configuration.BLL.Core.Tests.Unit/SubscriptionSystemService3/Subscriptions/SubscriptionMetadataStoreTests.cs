using System;
using System.Collections.Generic;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions
{
    [TestFixture]
    public sealed class SubscriptionMetadataStoreTests : TestFixtureBase
    {
        private ObjectChangingSubscription metadata;
        private ISubscriptionMetadataFinder subscriptionMetadataFinder;

        [SetUp]
        public void SetUp()
        {
            this.metadata = new ObjectChangingSubscription();

            this.subscriptionMetadataFinder = this.Fixture.RegisterStub<ISubscriptionMetadataFinder>();
            this.subscriptionMetadataFinder
                .Find()
                .Returns(new[] { this.metadata });
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(SubscriptionMetadataStore));
        }

        [Test]
        public void Get_DomainObjectType_SubscriptionMetadataCollection()
        {
            // Arrange
            var store = this.Fixture.Create<SubscriptionMetadataStore>();

            // Act
            var result = store.Get(this.metadata.DomainObjectType);
            var expected = new List<ISubscriptionMetadata> { this.metadata};

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Get_InvalidDomainObjectType_EmptySubscriptionMetadataCollection()
        {
            // Arrange
            var store = this.Fixture.Create<SubscriptionMetadataStore>();

            // Act
            var result = store.Get(typeof(string));

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void Ctor_InvalidMetadata_Exception()
        {
            // Arrange
            var subscription = new ObjectChangingSubscription();
            subscription.SetSenderName(null);

            var finder = Substitute.For<ISubscriptionMetadataFinder>();
            finder.Find().Returns(new[] { subscription });

            // Act
            Action call = () => new SubscriptionMetadataStore(finder);

            // Assert
            call.Should().Throw<SubscriptionModelingException>();
        }

        //[Test]
        //public void RegisterCodeFirstSubscriptions_Call_CodeFirstSubscriptionBllInvoked()
        //{
        //    // Arrange
        //    var subscriptionMetadata = new ObjectChangingSubscription();

        //    var domainType = this.Fixture.Create<DomainType>();

        //    var codeFirstSubscription = new CodeFirstSubscription(subscriptionMetadata.Code, domainType);

        //    var subscriptions = new[] { codeFirstSubscription };

        //    var finder = this.Fixture.RegisterStub<ISubscriptionMetadataFinder>();
        //    finder.Find().Returns(new[] { subscriptionMetadata });

        //    var context = this.CreateStub<IConfigurationBLLContext>();
        //    context.GetDomainType(subscriptionMetadata.DomainObjectType, true).Returns(domainType);

        //    var bll = this.CreateStrictMock<ICodeFirstSubscriptionBLL>();

        //    var store = this.Fixture.Create<SubscriptionMetadataStore>();

        //    // Act
        //    store.RegisterCodeFirstSubscriptions(bll, context);

        //    // Assert
        //    bll.Received().Save(subscriptions.AllPropertiesMatch());
        //}
    }
}
