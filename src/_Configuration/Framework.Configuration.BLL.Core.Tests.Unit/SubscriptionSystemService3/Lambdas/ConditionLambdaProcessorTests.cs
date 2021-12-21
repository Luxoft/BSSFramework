using System;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.ExpressionParsers;
using Framework.UnitTesting;

using NSubstitute.ExceptionExtensions;

using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Lambdas
{
    [TestFixture]
    public sealed class ConditionLambdaProcessorTests : TestFixtureBase
    {
        private IExpressionParserFactory parserFactory;

        [SetUp]
        public void SetUp()
        {
            this.parserFactory = new ExpressionParserFactory(CSharpNativeExpressionParser.Compile);
            this.Fixture.Register(() => this.parserFactory);
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(ConditionLambdaProcessor<ITestBLLContext>));
        }

        [Test]
        public void Invoke_WithoutContext_True()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");
            const string lambdaText = "(prev, current) => prev == \"a\" && current == \"b\"";

            // Act
            var invokeResult = this.Invoke(versions, lambdaText, string.Empty);

            // Assert
            invokeResult.Should().BeTrue();
        }

        [Test]
        public void Invoke_WithContext_True()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");
            const string lambdaText =
                "(context, prev, current) => context.Length == 5 && prev == \"a\" && current == \"b\"";

            // Act
            var invokeResult = this.Invoke(versions, lambdaText, "Hello");

            // Assert
            invokeResult.Should().BeTrue();
        }

        [Test]
        public void Invoke_FuncValue_True()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");

            var lambdaWrapper =
                new LambdaWrapper<string, string, bool>(
                    (context, vs) => context.Length == 5 && vs.Previous == "a" && vs.Current == "b");

            // Act
            var invokeResult = this.Invoke(versions, null, "Hello", lambdaWrapper.UntypedLambda);

            // Assert
            invokeResult.Should().BeTrue();
        }

        [Test]
        public void InvokeFuncValue_InvalidResultType_Exception()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");

            var lambdaWrapper =
                new LambdaWrapper<string, string, object>((context, vs) => "zzz");

            // Act
            Action call = ()=> this.Invoke(versions, null, "Hello", lambdaWrapper.UntypedLambda);

            // Assert
            call.Should().Throw<SubscriptionServicesException>()
                .WithMessage("Condition lambda for subscription \"S-000\" has failed with message \"Unable cast Condition lambda FuncValue result 'zzz' to required type System.Boolean\".");
        }

        [Test]
        public void Invoke_NoLambda_False()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");

            // Act
            var invokeResult = this.Invoke(versions, null, string.Empty);

            // Assert
            invokeResult.Should().BeFalse();
        }

        [Test]
        public void Invoke_NoPrevious_False()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>(null, "b");

            // Act
            var invokeResult = this.Invoke(versions, "(prev, current) => false", string.Empty);

            // Assert
            invokeResult.Should().BeFalse();
        }

        [Test]
        public void Invoke_NoCurrent_False()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", null);

            // Act
            var invokeResult = this.Invoke(versions, "(prev, current) => false", string.Empty);

            // Assert
            invokeResult.Should().BeFalse();
        }

        [Test]
        public void Invoke_Error_CorrectException()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");
            const string lambdaText ="(context, prev, current) => context.NonExistingProperty";

            // Act
            Action call = () => this.Invoke(versions, lambdaText, "Hello");

            // Assert
            call.Should().Throw<SubscriptionServicesException>();
        }

        private bool Invoke<T>(
            DomainObjectVersions<T> versions,
            string lambdaString,
            string context,
            Func<object, DomainObjectVersions<object>, object> funcValue = null) where T : class
        {
            // Arrange
            var lambda = this.Fixture
                .Build<SubscriptionLambda>()
                .With(l => l.Value, lambdaString)
                .With(l => l.FuncValue, funcValue)
                .With(l => l.RequiredModePrev, true)
                .With(l => l.RequiredModeNext, true)
                .With(c => c.WithContext, !string.IsNullOrEmpty(context))
                .Create();

            var subscription = this.Fixture
                .Build<Subscription>()
                .With(s => s.Code, "S-000")
                .With(s => s.Condition, lambdaString != null || funcValue != null ? lambda : null)
                .Create();

            // Act
            var processor = new ConditionLambdaProcessor<string>(context, this.parserFactory);
            return processor.Invoke(subscription, versions);
        }
    }
}
