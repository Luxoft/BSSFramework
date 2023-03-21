using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Templates;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Notification;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Services;

[TestFixture]
public sealed class NotificationServiceTests : TestFixtureBase
{
    private SubscriptionResolver subscriptionsResolver;
    private MessageTemplateFactory<ITestBLLContext> templateFactory;
    private IMessageSender<MessageTemplateNotification> templateSender;
    private ConfigurationContextFacade configurationContextFacade;

    [SetUp]
    public void SetUp()
    {
        this.subscriptionsResolver = this.Fixture.RegisterStub<SubscriptionResolver>();
        this.templateFactory = this.Fixture.RegisterStub<MessageTemplateFactory<ITestBLLContext>>();
        this.templateSender = this.Fixture.RegisterStub<IMessageSender<MessageTemplateNotification>>();
        this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();
    }

    [Test]
    public void PublicSurface_NullArguments_ArgumentNullException()
    {
        // Arrange
        var assertion = new GuardClauseAssertion(this.Fixture);

        // Act

        // Assert
        assertion.Verify(typeof(SubscriptionNotificationService<ITestBLLContext>));
    }

    [Test]
    public void NotifyDomainObjectChanged_NoErrors_EmptyTryResultCollection()
    {
        // Arrange
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();

        var subscriptions = this.Fixture.CreateMany<Subscription>(1).ToList();

        var template = new MessageTemplateNotification("test", this, this.GetType(), new string[0], new string[0], new string[0], null);

        this.templateSender.Send(template);

        this.subscriptionsResolver.Resolve(versions).Returns(subscriptions);

        // Act
        var service = this.Fixture.Create<SubscriptionNotificationService<ITestBLLContext>>();
        var results = service.NotifyDomainObjectChanged(versions);

        // Assert
        this.subscriptionsResolver.Received().Resolve(versions);

        this.templateFactory.Received().Create(Arg.Is<IEnumerable<Subscription>>(v => subscriptions.SequenceEqual(v)), versions);

        results.Should().HaveCount(0);
    }

    [Test]
    public void NotifyDomainObjectChanged_SubscriptionAndVersions_InnerComponentsCorrectInvoked()
    {
        // Arrange
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();

        var subscriptions = this.Fixture.CreateMany<Subscription>(1).ToList();
        var subscription = subscriptions.Single();

        var template = new MessageTemplateNotification("test", this, this.GetType(), new string[0], new string[0], new string[0], null, subscription);

        this.templateFactory.Create(Arg.Is<List<Subscription>>(v => v.SequenceEqual(subscriptions)), versions).Returns(new List<MessageTemplateNotification> { template });

        // Act
        var service = this.Fixture.Create<SubscriptionNotificationService<ITestBLLContext>>();
        service.NotifyDomainObjectChanged(subscription, versions);

        // Assert

        this.templateFactory.Received().Create(Arg.Is<List<Subscription>>(v => v.SequenceEqual(subscriptions)), versions);

        this.templateSender.Received().Send(template);
    }

    [Test]
    public void NotifyDomainObjectChanged_MessageTemplatesFactoryFailsOnCreate_ExceptionFromAggregate()
    {
        // Arrange
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();

        var subscriptions = this.Fixture.CreateMany<Subscription>(1).ToList();

        var exception = this.Fixture.Create<SubscriptionServicesException>();
        var aggregateException = new AggregateException(exception);

        this.subscriptionsResolver
            .Resolve(versions)
            .Returns(subscriptions);

        this.templateFactory
            .Create(Arg.Is<List<Subscription>>(v => v.SequenceEqual(subscriptions)), versions)
            .Throws(aggregateException);

        // Act
        var service = this.Fixture.Create<SubscriptionNotificationService<ITestBLLContext>>();
        var results = service.NotifyDomainObjectChanged(versions);
        var errors = results.GetErrors();

        // Assert
        errors.Single().Should().Be(exception);
    }

    [Test]
    public void NotifyDomainObjectChanged_MessageTemplatesFactoryFailsOnInput_Exception()
    {
        // Arrange
        var versions = this.Fixture.Create<DomainObjectVersions<string>>();

        this.subscriptionsResolver
            .Resolve(versions)
            .Returns(default(IEnumerable<Subscription>));

        // Act
        var service = this.Fixture.Create<SubscriptionNotificationService<ITestBLLContext>>();
        var results = service.NotifyDomainObjectChanged(versions);
        var errors = results.GetErrors();

        // Assert
        errors.Single().Should().BeOfType<ArgumentNullException>();
    }

    private void PrintResults(IEnumerable<ITryResult<Subscription>> results)
    {
        foreach (var tryResult in results)
        {
            if (tryResult.IsFault())
            {
                var error = ((IFaultResult<Subscription>)tryResult).Error;
                Console.WriteLine($"{error.Message}{Environment.NewLine}");
            }
        }
    }
}
