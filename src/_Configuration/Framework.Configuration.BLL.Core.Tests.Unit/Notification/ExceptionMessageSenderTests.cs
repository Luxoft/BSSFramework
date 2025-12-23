using System.Net.Mail;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.Notification.New;
using Framework.UnitTesting;
using Framework.Validation;

using NSubstitute;

using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.Notification;

[TestFixture]
public sealed class ExceptionMessageSenderTests : TestFixtureBase
{
    private static readonly MailAddress FromAddress = new MailAddress("support@luxoft.com");

    private static readonly string[] ToAddresses = { "user@luxoft.com" };

    private IAuthorizationBLLContext authorizationBLLContext;

    private IMessageSender<Message> messageSender;

    [SetUp]
    public void SetUp()
    {
        this.authorizationBLLContext = this.Fixture.RegisterStub<IAuthorizationBLLContext>();

        var configurationBLLContext = this.Fixture.RegisterStub<IConfigurationBLLContext>();
        configurationBLLContext.Authorization.Returns(this.authorizationBLLContext);

        this.messageSender = this.Fixture.RegisterStub<IMessageSender<Message>>();

        // register from address for class ctor
        this.Fixture.Register(() => FromAddress);

        // register recievers list for class ctor
        this.Fixture.Register<IEnumerable<string>>(() => ToAddresses);
    }

    [Test]
    public void PublicInterface_NullArguments_ArgumentNullException()
    {
        // Arrange
        var assertion = new GuardClauseAssertion(this.Fixture);

        // Act

        // Assert
        assertion.Verify(typeof(ExceptionMessageSender));
    }

    [Test]
    public async Task Send_LoginWithDomain_MessageWasSent()
    {
        // Arrange

        // Act

        // Assert
        await this.TestMessageWasSent(@"luxoft\John");
    }

    [Test]
    public async Task Send_LoginWithoutDomain_MessageWasSent()
    {
        // Arrange

        // Act

        // Assert
        await this.TestMessageWasSent(@"John");
    }

    [Test]
    public async Task Send_SomeException_CorrectMessageWasSent()
    {
        // Arrange
        var exception = this.Fixture.Create<ArgumentOutOfRangeException>();
        var sender = this.Fixture.Create<ExceptionMessageSender>();

        this.authorizationBLLContext.CurrentPrincipalSource.CurrentUser.Returns(new Principal { Name = @"luxoft\John" });

        Message sendedMessage = null;

        await this.messageSender.SendAsync(Arg.Do<Message>(m => sendedMessage = m));

        // Act
        await sender.SendAsync(exception);

        // Assert
        sendedMessage.Sender.Should().Be(FromAddress);
        sendedMessage.Receivers.Select(r => r.Address).Should().BeEquivalentTo(ToAddresses);
        sendedMessage.Subject.Should().Be("Exception (John) - ArgumentOutOfRangeException - Specified argument was out of");
        sendedMessage.Body.Should().Be(exception.ToFormattedString());
    }

    [Test]
    public async Task Send_InheritorSpecifiesExcludeExceptionType_MessageWasNotSent()
    {
        // Arrange
        var sender = this.Fixture.Create<TestingExceptionMessageSender>();
        sender.SetExceptTypes(typeof(InvalidOperationException));

        // Act
        await sender.SendAsync(
            this.Fixture.Create<InvalidOperationException>());

        // Assert
        await this.messageSender.DidNotReceive().SendAsync(Arg.Any<Message>());
    }

    [Test]
    public async Task Send_ValidationException_MessageWasNotSent()
    {
        // Arrange

        // Act

        // Assert
        await this.TestMessageWasNotSent(this.Fixture.Create<ValidationException>());
    }

    [Test]
    public async Task Send_AggregateValidationException_MessageWasNotSent()
    {
        // Arrange

        // Act

        // Assert
        await this.TestMessageWasNotSent(new AggregateValidationException([]));
    }

    private async Task TestMessageWasNotSent(Exception exception)
    {
        // Arrange
        var sender = this.Fixture.Create<ExceptionMessageSender>();

        // Act
        await sender.SendAsync(exception);

        // Assert
        await this.messageSender.DidNotReceive().SendAsync(Arg.Any<Message>());
    }

    private async Task TestMessageWasSent(string login)
    {
        // Arrange
        this.authorizationBLLContext.CurrentPrincipalSource.CurrentUser.Returns(new Principal { Name = login });

        var sender = this.Fixture.Create<ExceptionMessageSender>();

        // Act
        await sender.SendAsync(this.Fixture.Create<ArgumentOutOfRangeException>());

        // Assert
        await this.messageSender.DidNotReceive().SendAsync(Arg.Any<Message>());
    }

    public class TestingExceptionMessageSender : ExceptionMessageSender
    {
        private IEnumerable<Type> exceptTypes;

        public TestingExceptionMessageSender(
                IConfigurationBLLContext context,
                IMessageSender<Message> messageSender,
                MailAddress fromAddress,
                IEnumerable<string> toAddresses)
                : base(context, messageSender, fromAddress, toAddresses)
        {
        }

        public void SetExceptTypes(params Type[] exceptTypes)
        {
            this.exceptTypes = exceptTypes;
        }

        protected override IEnumerable<Type> GetExceptTypes()
        {
            return this.exceptTypes;
        }
    }
}
