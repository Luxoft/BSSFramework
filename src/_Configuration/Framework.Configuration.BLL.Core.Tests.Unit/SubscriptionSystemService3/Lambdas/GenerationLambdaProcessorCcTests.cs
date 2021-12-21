using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.ExpressionParsers;
using Framework.Notification;
using Framework.UnitTesting;

using NSubstitute.ExceptionExtensions;

using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Lambdas
{
    [TestFixture]
    public sealed class GenerationLambdaProcessorCcTests : TestFixtureBase
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
            assertion.Verify(typeof(GenerationLambdaProcessorCc<ITestBLLContext>));
        }

        [Test]
        public void Invoke_WithoutContext_NotificationMessageGenerationInfo()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");
            var lambdaText = @"(prev, current) => new List<NotificationMessageGenerationInfo>()
                                    {new  NotificationMessageGenerationInfo(""test@ya.ru"", current, prev)}";

            // Act
            var invokeResult = this.Invoke(versions, lambdaText, string.Empty);
            var info = invokeResult.Single();

            // Assert
            info.PreviousRoot.Should().Be(versions.Previous);
            info.CurrentRoot.Should().Be(versions.Current);
            info.Recipients.Single().Email.Should().Be("test@ya.ru");
        }

        [Test]
        public void Invoke_WithContext_NotificationMessageGenerationInfo()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");
            var lambdaText = @"(context, prev, current) =>new List<NotificationMessageGenerationInfo>()
                                      {new  NotificationMessageGenerationInfo(context + ""@ya.ru"", current, prev)}";

            // Act
            var invokeResult = this.Invoke(versions, lambdaText, "test");
            var info = invokeResult.Single();

            // Assert
            info.PreviousRoot.Should().Be(versions.Previous);
            info.CurrentRoot.Should().Be(versions.Current);
            info.Recipients.Single().Email.Should().Be("test@ya.ru");
        }

        [Test]
        public void Invoke_FuncValue_NotificationMessageGenerationInfo()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");

            var lambdaWrapper =
                new LambdaWrapper<string, string, IEnumerable<NotificationMessageGenerationInfo>>(
                    (context, vs) =>
                        new List<NotificationMessageGenerationInfo>()
                        {
                            new NotificationMessageGenerationInfo(context + "@ya.ru", vs.Current, vs.Previous)
                        });

            // Act
            var invokeResult = this.Invoke(versions, null, "test", lambdaWrapper.UntypedLambda);
            var info = invokeResult.Single();

            // Assert
            info.PreviousRoot.Should().Be(versions.Previous);
            info.CurrentRoot.Should().Be(versions.Current);
            info.Recipients.Single().Email.Should().Be("test@ya.ru");
        }

        [Test]
        public void InvokeFuncValue_InvalidResultType_Exception()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");

            var lambdaWrapper =
                new LambdaWrapper<string, string, object>((context, vs) => "zzz");

            // Act
            Action call = () => this.Invoke(versions, null, "Hello", lambdaWrapper.UntypedLambda);

            // Assert
            call.Should().Throw<SubscriptionServicesException>()
                .WithMessage("CopyGeneration lambda for subscription \"S-000\" has failed with message \"Unable cast CopyGeneration lambda FuncValue result 'zzz' to required type System.Collections.Generic.IEnumerable`1[Framework.Notification.NotificationMessageGenerationInfo]\".");
        }

        [Test]
        public void Invoke_NoLambda_EmptyResult()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>(null, "b");

            // Act
            var invokeResult = this.Invoke(versions, null, string.Empty);

            // Assert
            invokeResult.Should().HaveCount(0);
        }

        [Test]
        public void Invoke_NoPrevious_EmptyResult()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>(null, "b");

            // Act
            var invokeResult = this.Invoke(versions, "(prev, current) => null", string.Empty);

            // Assert
            invokeResult.Should().HaveCount(0);
        }

        [Test]
        public void Invoke_NoCurrent_EmptyResult()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", null);

            // Act
            var invokeResult = this.Invoke(versions, "(prev, current) => null", string.Empty);

            // Assert
            invokeResult.Should().HaveCount(0);
        }

        [Test]
        public void Invoke_Error_CorrectException()
        {
            // Arrange
            var versions = new DomainObjectVersions<string>("a", "b");
            const string lambdaText = "(context, prev, current) => context.NonExistingProperty";

            // Act
            Action call = () => this.Invoke(versions, lambdaText, "Hello");

            // Assert
            call.Should().Throw<SubscriptionServicesException>();
        }

        private IEnumerable<NotificationMessageGenerationInfo> Invoke<T>(
            DomainObjectVersions<T> versions,
            string lambdaString,
            string context,
            Func<object, object, object> funcValue = null) where T : class
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
                .With(s => s.CopyGeneration, lambdaString != null || funcValue != null ? lambda : null)
                .Create();

            // Act
            var processor = new GenerationLambdaProcessorCc<string>(context, this.parserFactory);
            return processor.Invoke(subscription, versions);
        }
    }
}
