using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Notification;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Recipients
{
    [TestFixture]
    public sealed class GenerationRecipientsResolverTests : TestFixtureBase
    {
        private GenerationLambdaProcessorTo<ITestBLLContext> toProcessor;
        private GenerationLambdaProcessorCc<ITestBLLContext> ccProcessor;
        private GenerationLambdaProcessorReplyTo<ITestBLLContext> replayToProcessor;

        [SetUp]
        public void SetUp()
        {
            this.toProcessor = this.Fixture.RegisterStub<GenerationLambdaProcessorTo<ITestBLLContext>>();
            this.ccProcessor = this.Fixture.RegisterStub<GenerationLambdaProcessorCc<ITestBLLContext>>();
            this.replayToProcessor = this.Fixture.RegisterStub<GenerationLambdaProcessorReplyTo<ITestBLLContext>>();

            var processorFactory = this.Fixture.RegisterStub<LambdaProcessorFactory<ITestBLLContext>>();
            processorFactory.Create<GenerationLambdaProcessorTo<ITestBLLContext>>().Returns(this.toProcessor);
            processorFactory.Create<GenerationLambdaProcessorCc<ITestBLLContext>>().Returns(this.ccProcessor);
            processorFactory.Create<GenerationLambdaProcessorReplyTo<ITestBLLContext>>().Returns(this.replayToProcessor);
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(GenerationRecipientsResolver<ITestBLLContext>));
        }

        [Test]
        public void Resolver_Call_RecipientsResolverResultСollection()
        {
            // Arrange
            var toRecipient = new Recipient("ivanov", "ivanov@mail.ru");
            var ccRecipient = new Recipient("petrov", "petrov@mail.ru");
            var replayToRecipient = new Recipient("replayTo", "replayTo@mail.ru");

            var versions = this.Fixture.Create<DomainObjectVersions<string>>();
            var subscription = this.Fixture.Create<Subscription>();

            var infoTo = new NotificationMessageGenerationInfo(toRecipient, versions.Current, versions.Previous);
            var infoCc = new NotificationMessageGenerationInfo(ccRecipient, versions.Current, versions.Previous);
            var replyTo = new NotificationMessageGenerationInfo(replayToRecipient, versions.Current, versions.Previous);


            this.toProcessor.Invoke(subscription, versions).Returns(new[] { infoTo });
            this.ccProcessor.Invoke(subscription, versions).Returns(new[] { infoCc });
            this.replayToProcessor.Invoke(subscription, versions).Returns(new[] { replyTo });

            // Act
            var resolver = this.Fixture.Create<GenerationRecipientsResolver<ITestBLLContext>>();
            var resolverResults = resolver.Resolve(subscription, versions);
            var toResult = resolverResults.First();
            var ccResult = resolverResults.Last();
            var replayToResult = resolverResults.SelectMany(z => z.RecipientsBag.ReplyTo).ToList();

            // Assert
            toResult.RecipientsBag.To.First().Email.Should().Be(toRecipient.Email);
            ccResult.RecipientsBag.Cc.First().Email.Should().Be(ccRecipient.Email);
            replayToResult.Should().BeEquivalentTo(new[] { replayToRecipient });
        }

        [Test]
        public void Resolver_CallWithEmpty_RecipientsResolverResultСollection()
        {
            // Arrange
            var toRecipient = new Recipient("ivanov", "ivanov@mail.ru");
            var ccRecipient = new Recipient("petrov", "petrov@mail.ru");

            var versions = this.Fixture.Create<DomainObjectVersions<string>>();
            var subscription = this.Fixture.Create<Subscription>();

            var infoTo = new NotificationMessageGenerationInfo(toRecipient, versions.Current, versions.Previous);
            var infoCc = new NotificationMessageGenerationInfo(ccRecipient, versions.Current, versions.Previous);


            this.toProcessor.Invoke(subscription, versions).Returns(new[] { infoTo });
            this.ccProcessor.Invoke(subscription, versions).Returns(new[] { infoCc });
            this.replayToProcessor.Invoke(subscription, versions).Returns(new NotificationMessageGenerationInfo[0]);

            // Act
            var resolver = this.Fixture.Create<GenerationRecipientsResolver<ITestBLLContext>>();
            var resolverResults = resolver.Resolve(subscription, versions);
            var toResult = resolverResults.First();
            var ccResult = resolverResults.Last();
            var replyToResult = resolverResults.SelectMany(z => z.RecipientsBag.ReplyTo).ToList();

            // Assert
            toResult.RecipientsBag.To.First().Email.Should().Be(toRecipient.Email);
            ccResult.RecipientsBag.Cc.First().Email.Should().Be(ccRecipient.Email);
            replyToResult.Should().BeEmpty();
        }

    }
}
