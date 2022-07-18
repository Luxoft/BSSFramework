using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Validation;

using JetBrains.Annotations;

using Attachment = Framework.Notification.New.Attachment;

namespace Framework.Configuration.BLL.Notification
{
    /// <summary>
    /// Базовый класс для рассылки уведомлений по исключениям.
    /// </summary>
    /// <seealso cref="DomainDriven.BLL.BLLContextContainer{IConfigurationBLLContext}" />
    /// <seealso cref="Framework.Core.IMessageSender{Exception}" />
    public class ExceptionMessageSender : BLLContextContainer<IConfigurationBLLContext>, IMessageSender<Exception>
    {
        private readonly IMessageSender<Framework.Notification.New.Message> messageSender;
        private readonly MailAddress fromAddress;
        private readonly string[] receivers;

        private readonly IEnumerable<Type> exceptTypes =
            new[]
            {
                typeof(ValidationException),
                typeof(AggregateValidationException),
            };

        /// <summary>
        /// Создаёт экземпляр класса <see cref="ExceptionMessageSender"/>.
        /// </summary>
        /// <param name="context">Контекст конфигурации.</param>
        /// <param name="messageSender">Оборачиваемый экземпляр класса рассылки уведомлений.</param>
        /// <param name="fromAddress">Адрес отправителя сообщения.</param>
        /// <param name="toAddresses">Адреса получателей сообщения.</param>
        /// <exception cref="ArgumentNullException">Аргумент
        /// <paramref name="context"/>
        /// или
        /// <paramref name="fromAddress"/>
        /// или
        /// <paramref name="toAddresses"/> равен null.
        /// </exception>
        public ExceptionMessageSender(
            [NotNull] IConfigurationBLLContext context,
            [NotNull] IMessageSender<Framework.Notification.New.Message> messageSender,
            [NotNull] MailAddress fromAddress,
            [NotNull] IEnumerable<string> toAddresses)
            : base(context)
        {
            if (toAddresses == null)
            {
                throw new ArgumentNullException(nameof(toAddresses));
            }

            this.fromAddress = fromAddress ?? throw new ArgumentNullException(nameof(fromAddress));
            this.messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            this.receivers = toAddresses.ToArray();

            if (!this.receivers.Any())
            {
                throw new System.ArgumentException("Collection 'Receivers' is empty", nameof(this.receivers));
            }
        }

        /// <summary>
        /// Осуществляет отправку уведомления об исключении.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <exception cref="ArgumentNullException">Аргумент <paramref name="exception"/> равен null.</exception>
        public void Send(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (this.SkipSend(exception))
            {
                return;
            }

            var userLogin = this.Context.Authorization.CurrentPrincipalName?.Split('\\').Last() ?? string.Empty;
            var baseType = exception.GetBaseException().GetType();
            var exceptionName = baseType.Name;
            var messagePart = exception.Message;

            var subject = $"Exception ({userLogin}) - {exceptionName} - {messagePart}";
            var body = exception.ToFormattedString();

            var message = new Framework.Notification.New.Message(
                this.fromAddress,
                this.receivers,
                subject,
                body,
                false,
                new Attachment[0]);

            this.messageSender.Send(message);
        }

        /// <summary>
        /// Определяет разрешить или нет отправку уведомления по исключению.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <returns>Флаг, определяющий разрешить или нет отправку уведомления по исключению.</returns>
        protected virtual bool SkipSend(Exception exception)
        {
            return this.GetExceptTypes().Contains(exception.GetBaseException().GetType());
        }

        /// <summary>
        /// Возвращает список типов Exception, исключаемых из рассылки.
        /// </summary>
        /// <returns>список типов Exception, исключаемых из рассылки.</returns>
        protected virtual IEnumerable<Type> GetExceptTypes()
        {
            return this.exceptTypes;
        }
    }
}
