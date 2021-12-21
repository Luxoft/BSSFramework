using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.Notification.New;
using Framework.UnitTesting;
using Framework.Validation;

using JetBrains.Annotations;

using NSubstitute;

using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.Notification
{
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
        public void Send_LoginWithDomain_MessageWasSent()
        {
            // Arrange

            // Act

            // Assert
            this.TestMessageWasSent(@"luxoft\John");
        }

        [Test]
        public void Send_LoginWithoutDomain_MessageWasSent()
        {
            // Arrange

            // Act

            // Assert
            this.TestMessageWasSent(@"John");
        }

        [Test]
        public void Send_SomeException_CorrectMessageWasSent()
        {
            // Arrange
            var exception = this.Fixture.Create<ArgumentOutOfRangeException>();
            var sender = this.Fixture.Create<ExceptionMessageSender>();

            this.authorizationBLLContext
                .CurrentPrincipalName
                .Returns(@"luxoft\John");

            Message sendedMessage = null;

            this.messageSender.Send(Arg.Do<Message>(m => sendedMessage = m), TransactionMessageMode.Auto);

            // Act
            sender.Send(
                exception,
                this.Fixture.Create<TransactionMessageMode>());

            // Assert
            sendedMessage.Sender.Should().Be(FromAddress);
            sendedMessage.Receivers.Select(r => r.Address).Should().BeEquivalentTo(ToAddresses);
            sendedMessage.Subject.Should().Be("Exception (John) - ArgumentOutOfRangeException - Specified argument was out of");
            sendedMessage.Body.Should().Be(exception.ToFormattedString());
        }

        [Test]
        public void Send_InheritorSpecifiesExcludeExceptionType_MessageWasNotSent()
        {
            // Arrange
            var sender = this.Fixture.Create<TestingExceptionMessageSender>();
            sender.SetExceptTypes(typeof(InvalidOperationException));

            // Act
            sender.Send(
                this.Fixture.Create<InvalidOperationException>(),
                this.Fixture.Create<TransactionMessageMode>());

            // Assert
            this.messageSender.DidNotReceive().Send(Arg.Any<Message>(), TransactionMessageMode.Auto);
        }

        [Test]
        public void Send_ValidationException_MessageWasNotSent()
        {
            // Arrange

            // Act

            // Assert
            this.TestMessageWasNotSent(this.Fixture.Create<ValidationException>());
        }

        [Test]
        public void Send_AggregateValidationException_MessageWasNotSent()
        {
            // Arrange

            // Act

            // Assert
            this.TestMessageWasNotSent(new AggregateValidationException(Enumerable.Empty<ValidationExceptionBase>()));
        }

        private void TestMessageWasNotSent(Exception exception)
        {
            // Arrange
            var sender = this.Fixture.Create<ExceptionMessageSender>();

            // Act
            sender.Send(exception, this.Fixture.Create<TransactionMessageMode>());

            // Assert
            this.messageSender.DidNotReceive().Send(Arg.Any<Message>(), TransactionMessageMode.Auto);
        }

        private void TestMessageWasSent(string login)
        {
            // Arrange
            this.authorizationBLLContext
                .CurrentPrincipalName
                .Returns(login);

            var sender = this.Fixture.Create<ExceptionMessageSender>();

            // Act
            sender.Send(
                this.Fixture.Create<ArgumentOutOfRangeException>(),
                this.Fixture.Create<TransactionMessageMode>());

            // Assert
            this.messageSender.DidNotReceive().Send(Arg.Any<Message>(), TransactionMessageMode.Auto);
        }

        public class TestingExceptionMessageSender : ExceptionMessageSender
        {
            private IEnumerable<Type> exceptTypes;

            public TestingExceptionMessageSender(
                [NotNull] IConfigurationBLLContext context,
                [NotNull] IMessageSender<Message> messageSender,
                [NotNull] MailAddress fromAddress,
                [NotNull] IEnumerable<string> toAddresses)
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
}
