using AutoFixture;

using FluentAssertions;

using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.UnitTesting;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Lambdas
{
    [TestFixture]
    public sealed class LambdaProcessorTests : TestFixtureBase
    {
        [Test]
        [TestCase(null, null, "A", "B", TestName = "Any Event")]
        [TestCase(false, true, null, "B", TestName = "Create / Create")]
        [TestCase(true, true, "A", "B", TestName = "Update / Update")]
        [TestCase(true, false, "A", null, TestName = "Delete / Delete")]
        [TestCase(null, true, null, "B", TestName = "CreateOrUpdate / Create")]
        [TestCase(null, true, "A", "B", TestName = "CreateOrUpdate / Update")]
        [TestCase(true, null, "A", "B", TestName = "UpdateOrDelete / Update")]
        [TestCase(true, null, "A", null, TestName = "UpdateOrDelete / Delete")]
        public void DomainObjectCompliesLambdaRequiredMode_MiscValidArgsVariants_True(
                bool? requiredModePrev,
                bool? requiredModeNext,
                object previous,
                object current)
        {
            // Arrange
            var processor = this.Fixture.Create<TestLambdaProcessor>();

            var lambda = new SubscriptionLambda();
            lambda.RequiredModePrev = requiredModePrev;
            lambda.RequiredModeNext = requiredModeNext;

            var versions = new DomainObjectVersions<object>(previous, current);

            // Act
            var complies = processor.GetDomainObjectCompliesLambdaRequiredModeResult(lambda, versions);

            // Assert
            complies.Should().BeTrue();
        }

        [Test]
        public void DomainObjectCompliesLambdaRequiredMode_NullLambda_False()
        {
            // Arrange
            var processor = this.Fixture.Create<TestLambdaProcessor>();
            var versions = this.Fixture.Create<DomainObjectVersions<object>>();

            // Act
            var complies = processor.GetDomainObjectCompliesLambdaRequiredModeResult(null, versions);

            // Assert
            complies.Should().BeFalse();
        }

        [Test]
        public void DomainObjectCompliesLambdaRequiredMode_NullVersions_False()
        {
            // Arrange
            var processor = this.Fixture.Create<TestLambdaProcessor>();
            var lambda = new SubscriptionLambda();

            // Act
            var complies = processor.GetDomainObjectCompliesLambdaRequiredModeResult<object>(lambda, null);

            // Assert
            complies.Should().BeFalse();
        }

        public class TestLambdaProcessor : LambdaProcessor<ITestBLLContext>
        {
            public TestLambdaProcessor([NotNull] ITestBLLContext bllContext)
                : base(bllContext)
            {
            }

            protected override string LambdaName { get; } = "TestLambdaProcessor";

            public bool GetDomainObjectCompliesLambdaRequiredModeResult<T>(
                    SubscriptionLambda lambda,
                    DomainObjectVersions<T> versions)
                where T : class
            {
                return DomainObjectCompliesLambdaRequiredMode(lambda, versions);
            }
        }
    }
}
