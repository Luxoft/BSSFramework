using System;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Services;

[TestFixture]
public class SubscriptionServicesFactoryTests : TestFixtureBase
{
    [SetUp]
    public void SetUp()
    {
        var messageSender = this.Fixture.RegisterStub<IMessageSender<MessageTemplateNotification>>();

        var context = this.Fixture.RegisterStub<IConfigurationBLLContext>();
        context.SubscriptionSender.Returns(messageSender);

        var defaultBllFactory = this.Fixture.RegisterStub<IDefaultBLLFactory<IdentityObject, Guid>>();
        defaultBllFactory
                .Create<IdentityObject>()
                .Returns(this.CreateStub<IDefaultDomainBLLBase<IdentityObject, IdentityObject, Guid>>());
    }

    [Test]
    public void PublicSurface_NullArguments_ArgumentNullException()
    {
        // Arrange
        var assertion = new GuardClauseAssertion(this.Fixture);

        // Act

        // Assert
        assertion.Verify(typeof(SubscriptionServicesFactory<ITestBLLContext, IdentityObject>).GetConstructors());
    }

    [Test]
    public void CreateNotificationService_Call_ServiceInstance()
    {
        // Arrange
        var factory = this.Fixture.Create<SubscriptionServicesFactory<ITestBLLContext, IdentityObject>>();

        // Act
        var service = factory.CreateNotificationService();

        // Assert
        service.Should().NotBeNull();
    }

    [Test]
    public void CreateRecipientService_Call_ServiceInstance()
    {
        // Arrange
        var factory = this.Fixture.Create<SubscriptionServicesFactory<ITestBLLContext, IdentityObject>>();

        // Act
        var service = factory.CreateRecipientService();

        // Assert
        service.Should().NotBeNull();
    }

    [Test]
    public void CreateRevisionService_Call_ServiceInstance()
    {
        // Arrange
        var factory = this.Fixture.Create<SubscriptionServicesFactory<ITestBLLContext, IdentityObject>>();

        // Act
        var service = factory.CreateRevisionService<IdentityObject>();

        // Assert
        service.Should().NotBeNull();
    }
        
    [Test]
    public void CreateRevisionServiceByType_Call_ServiceInstance()
    {
        // Arrange
        var factory = this.Fixture.Create<SubscriptionServicesFactory<ITestBLLContext, IdentityObject>>();

        // Act
        var service = factory.CreateRevisionService(typeof(IdentityObject));

        // Assert
        service.Should().NotBeNull();
        service.Should().BeAssignableTo<RevisionService<IdentityObject>>();
    }

    [Test]
    public void CreateRevisionServiceByType_CallOnBaseFactoryInstance_Exception()
    {
        // Arrange
        var factory = this.Fixture.Create<SubscriptionServicesFactory<ITestBLLContext>>();

        // Act
        Action call = () => factory.CreateRevisionService(typeof(IdentityObject));

        // Assert
        call.Should().Throw<NotSupportedException>();
    }

    [Test]
    public void CreateLambdaProcessor_Call_ProcessorInstance()
    {
        // Arrange
        var factory = this.Fixture.Create<SubscriptionServicesFactory<ITestBLLContext, IdentityObject>>();

        // Act
        var processor = factory.CreateLambdaProcessor<ConditionLambdaProcessor<ITestBLLContext>>();

        // Assert
        processor.Should().NotBeNull();
    }
}
