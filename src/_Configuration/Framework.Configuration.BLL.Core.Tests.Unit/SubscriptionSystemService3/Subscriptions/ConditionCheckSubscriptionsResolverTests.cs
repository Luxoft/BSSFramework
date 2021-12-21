using System;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;

using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
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
    public sealed class ConditionCheckSubscriptionsResolverTests : TestFixtureBase
    {
        private SubscriptionResolver innerResolver;
        private ConditionLambdaProcessor<ITestBLLContext> lambdaProcessor;

        [SetUp]
        public void SetUp()
        {
            this.innerResolver = this.Fixture.RegisterStub<SubscriptionResolver>();
            this.lambdaProcessor = this.Fixture.RegisterStub<ConditionLambdaProcessor<ITestBLLContext>>();

            var processorFactory = this.Fixture.RegisterStub<LambdaProcessorFactory<ITestBLLContext>>();
            processorFactory
                .Create<ConditionLambdaProcessor<ITestBLLContext>>()
                .Returns(this.lambdaProcessor);
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(ConditionCheckSubscriptionsResolver<ITestBLLContext>));
        }

        [Test]
        public void Resolve_Call_ResultFilteredByCondition()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var subscription1 = this.Fixture.Build<Subscription>().With(s => s.Code, "s_1").Create();
            var subscription2 = this.Fixture.Build<Subscription>().With(s => s.Code, "s_2").Create();

            this.innerResolver.Resolve(versions).Returns(new[] { subscription1, subscription2 });

            this.lambdaProcessor.Invoke(subscription1, versions).Returns(false);
            this.lambdaProcessor.Invoke(subscription2, versions).Returns(true);

            // Act
            var resolver = this.Fixture.Create<ConditionCheckSubscriptionsResolver<ITestBLLContext>>();
            var callResult = resolver.Resolve(versions).ToList();

            // Assert
            callResult.Should().HaveCount(1);
            callResult.First().Should().Be(subscription2);
        }

        [Test]
        public void Resolve_Call_ConditionFails_SubscriptionExcludedFromResult()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var subscription1 = this.Fixture.Build<Subscription>().With(s => s.Code, "s_1").Create();
            var subscription2 = this.Fixture.Build<Subscription>().With(s => s.Code, "s_2").Create();

            this.innerResolver.Resolve(versions).Returns(new[] { subscription1, subscription2 });

            this.lambdaProcessor.Invoke(subscription1, versions).Returns(true);
            this.lambdaProcessor.Invoke(subscription2, versions).Throws(new Exception());

            // Act
            var resolver = this.Fixture.Create<ConditionCheckSubscriptionsResolver<ITestBLLContext>>();
            var callResult = resolver.Resolve(versions).ToList();

            // Assert
            callResult.Should().HaveCount(1);
            callResult.First().Should().Be(subscription1);
        }

        [Test]
        public void ResolveBySubscriptionCode_Call_Subscription()
        {
            // Arrange
            var subscriptionCode = this.Fixture.Create<string>();
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();
            var subscription = this.Fixture.Create<Subscription>();

            this.innerResolver
                .Resolve(subscriptionCode, versions)
                .Returns(subscription);

            // Act
            var resolver = this.Fixture.Create<ConditionCheckSubscriptionsResolver<ITestBLLContext>>();
            var result = resolver.Resolve(subscriptionCode, versions);

            // Assert
            result.Should().BeSameAs(subscription);
        }

        [Test]
        public void IsActiveSubscriptionForTypeExists_Call_True()
        {
            // Arrange
            var type = this.GetType();

            this.innerResolver
                .IsActiveSubscriptionForTypeExists(type)
                .Returns(true);

            // Act
            var resolver = this.Fixture.Create<ConditionCheckSubscriptionsResolver<ITestBLLContext>>();
            var result = resolver.IsActiveSubscriptionForTypeExists(type);

            // Assert
            result.Should().BeTrue();
        }
    }
}
