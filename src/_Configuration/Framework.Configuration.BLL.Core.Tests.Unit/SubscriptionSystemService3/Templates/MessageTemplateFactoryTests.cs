using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.BLL.SubscriptionSystemService3.Templates;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Notification;
using Framework.UnitTesting;
using NUnit.Framework;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Templates
{
    [TestFixture]
    public sealed class MessageTemplateFactoryTests : TestFixtureBase
    {
        private Subscription subscription;
        private RecipientCollection to;
        private RecipientCollection cc;
        private RecipientCollection replyTo;
        private DomainObjectVersions<string> domainObjectVersions;
        private DomainObjectVersions<object> untypedDomainObjectVersions;
        private RecipientsResolver<ITestBLLContext> recipientsResolver;
        private ConfigurationContextFacade configurationContextFacade;

        [SetUp]
        public void SetUp()
        {
            this.subscription = this.Fixture.Create<Subscription>();

            this.to = new RecipientCollection(
                new[] { this.Fixture.Create<Recipient>(), this.Fixture.Create<Recipient>() });

            this.cc = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

            this.replyTo = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

            this.domainObjectVersions = new DomainObjectVersions<string>("1", "2");

            this.untypedDomainObjectVersions = new DomainObjectVersions<object>(
                this.domainObjectVersions.Previous,
                this.domainObjectVersions.Current);

            this.recipientsResolver = this.Fixture.RegisterStub<RecipientsResolver<ITestBLLContext>>();

            this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();

            var templatesFilter = this.Fixture.RegisterStub<ExcessTemplatesFilter>();

            templatesFilter.FilterTemplates(Arg.Any<IEnumerable<MessageTemplateNotification>>()).Returns(c => c.ArgAt<IEnumerable<MessageTemplateNotification>>(0));
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(MessageTemplateFactory<ITestBLLContext>));
        }

        [Test]
        public void Create_NoCc_SendIndividualLetters_TemplateCreatedForEachToReciptient()
        {
            // Arrange
            var recipients = new RecipientsBag(this.to, new RecipientCollection(), this.replyTo);
            var resolverResult = new RecipientsResolverResult(recipients, this.untypedDomainObjectVersions);

            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();
            this.subscription.SendIndividualLetters = true;

            this.recipientsResolver
                .Resolve(this.subscription, this.domainObjectVersions)
                .Returns(new[] { resolverResult });

            // Act
            var templates = factory.Create(new []{ this.subscription }, this.domainObjectVersions);

            // Asssert
            templates.Should().HaveCount(2);
        }

        [Test]
        public void Create_NoCc_NoSendIndividualLetters_TemplateCreatedForEachToReciptient()
        {
            // Arrange
            var recipients = new RecipientsBag(this.to, new RecipientCollection(), this.replyTo);
            var resolverResult = new RecipientsResolverResult(recipients, this.untypedDomainObjectVersions);
            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();

            this.recipientsResolver
                .Resolve(this.subscription, this.domainObjectVersions)
                .Returns(new[] { resolverResult });

            // Act
            var templates = factory.Create(new []{ this.subscription }, this.domainObjectVersions);

            // Asssert
            templates.Should().HaveCount(1);
        }

        [Test]
        public void Create_WithCc_OneTemplateCreatedForAll()
        {
            // Arrange
            var recipients = this.CreateRecipients();
            var resolverResult = new RecipientsResolverResult(recipients, this.untypedDomainObjectVersions);
            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();

            this.recipientsResolver
                .Resolve(this.subscription, this.domainObjectVersions)
                .Returns(new[] { resolverResult });

            // Act
            var templates = factory.Create(new[] { this.subscription }, this.domainObjectVersions);

            // Asssert
            templates.Should().HaveCount(1);
        }

        [Test]
        public void Create_WithCc_CorrectTemplateCreated()
        {
            // Arrange
            var recipients = this.CreateRecipients();
            var resolverResult = new RecipientsResolverResult(recipients, this.untypedDomainObjectVersions);
            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();

            this.recipientsResolver
                .Resolve(this.subscription, this.domainObjectVersions)
                .Returns(new[] { resolverResult });

            // Act
            var template = factory.Create(new[] { this.subscription }, this.domainObjectVersions).Single();

            // Asssert
            template.Receivers.Should().BeEquivalentTo(this.to.Select(r => r.Email));
            template.CopyReceivers.Should().BeEquivalentTo(this.cc.Select(r => r.Email));
            template.ReplyTo.Should().BeEquivalentTo(this.replyTo.Select(r => r.Email));
            template.Subscription.Should().Be(this.subscription);
            template.SendWithEmptyListOfRecipients.Should().Be(this.subscription.AllowEmptyListOfRecipients);
            template.ContextObject.Should().Be(this.domainObjectVersions);
            template.ContextObjectType.Should().Be(this.domainObjectVersions.DomainObjectType);
            template.RazorMessageTemplateType.Should().Be(this.subscription.RazorMessageTemplateType);
        }

        [Test]
        public void Create_WithCcWIthReplayToEmpty_CorrectTemplateCreated()
        {
            // Arrange
            this.replyTo = new RecipientCollection();
            var recipients = this.CreateRecipients();
            var resolverResult = new RecipientsResolverResult(recipients, this.untypedDomainObjectVersions);
            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();

            this.recipientsResolver
                .Resolve(this.subscription, this.domainObjectVersions)
                .Returns(new[] { resolverResult });

            // Act
            var template = factory.Create(new[] { this.subscription }, this.domainObjectVersions).Single();

            // Asssert
            template.Receivers.Should().BeEquivalentTo(this.to.Select(r => r.Email));
            template.CopyReceivers.Should().BeEquivalentTo(this.cc.Select(r => r.Email));
            template.ReplyTo.Should().BeEquivalentTo(new List<string>());
            template.Subscription.Should().Be(this.subscription);
            template.SendWithEmptyListOfRecipients.Should().Be(this.subscription.AllowEmptyListOfRecipients);
            template.ContextObject.Should().Be(this.domainObjectVersions);
            template.ContextObjectType.Should().Be(this.domainObjectVersions.DomainObjectType);
            template.RazorMessageTemplateType.Should().Be(this.subscription.RazorMessageTemplateType);
        }

        [Test]
        public void Create_WithoutCc_CorrectTemplateCreated()
        {
            // Arrange
            var recipients = new RecipientsBag(this.to, new RecipientCollection(), this.replyTo);
            var resolverResult = new RecipientsResolverResult(recipients, this.untypedDomainObjectVersions);
            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();

            this.subscription.SendIndividualLetters = true;
            this.recipientsResolver
                .Resolve(this.subscription, this.domainObjectVersions)
                .Returns(new[] { resolverResult });

            // Act
            var template = factory.Create(new[] { this.subscription }, this.domainObjectVersions).First();

            // Asssert
            template.Receivers.Should().BeEquivalentTo(this.to.First().Email);
            template.CopyReceivers.Should().BeEmpty();
            template.Subscription.Should().Be(this.subscription);
            template.SendWithEmptyListOfRecipients.Should().Be(this.subscription.AllowEmptyListOfRecipients);
            template.ContextObject.Should().Be(this.domainObjectVersions);
            template.RazorMessageTemplateType.Should().Be(this.subscription.RazorMessageTemplateType);
        }

        [Test]
        public void Create_SomethingFailsOnTemplatesCreation_AggregateException()
        {
            // Arrange
            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();
            var exception = new SubscriptionServicesException();

            this.recipientsResolver
                .Resolve(Arg.Any<Subscription>(), this.domainObjectVersions)
                .Throws(exception);

            // Act
            Action call = () => factory.Create(new[] { this.subscription }, this.domainObjectVersions);

            // Asssert
            call.Should().Throw<AggregateException>().Where(ae => ae.InnerExceptions.Contains(exception));
        }

        [Test]
        public void Create_DomainObjectVersionsRewrited_TemplatesWithRewritedDomainObjects()
        {
            // Arrange
            var objectVersions = new DomainObjectVersions<object>(new Version("1.0.0.0"), new Version("2.0.0.0"));
            var expectedVersions = new DomainObjectVersions<Version>(new Version("1.0.0.0"), new Version("2.0.0.0"));

            var recipients = this.CreateRecipients();
            var resolverResult = new RecipientsResolverResult(recipients, objectVersions);
            var factory = this.Fixture.Create<MessageTemplateFactory<ITestBLLContext>>();

            this.recipientsResolver
                .Resolve(this.subscription, this.domainObjectVersions)
                .Returns(new[] { resolverResult });

            // Act
            var template = factory.Create(new[] { this.subscription }, this.domainObjectVersions).Single();

            // Asssert
            template.ContextObject.Should().Be(expectedVersions);
            template.ContextObjectType.Should().Be(typeof(Version));
        }

        private RecipientsBag CreateRecipients() => new RecipientsBag(this.to, this.cc, this.replyTo);
    }
}
