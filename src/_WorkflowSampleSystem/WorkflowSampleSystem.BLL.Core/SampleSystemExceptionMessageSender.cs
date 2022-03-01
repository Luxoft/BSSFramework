using System;
using System.Collections.Generic;
using System.Net.Mail;

using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.Notification.New;

namespace WorkflowSampleSystem.BLL
{
    /// <summary>
    ///     Класс для рассылки уведомлений по исключениям в WorkflowSampleSystem.
    /// </summary>
    /// <seealso cref="Framework.Configuration.BLL.Notification.ExceptionMessageSender" />
    public sealed class WorkflowSampleSystemExceptionMessageSender : ExceptionMessageSender
    {
        /// <summary>
        ///     Создаёт экземпляр класса <see cref="WorkflowSampleSystemExceptionMessageSender" />.
        /// </summary>
        /// <param name="context">Контекст конфигурации.</param>
        /// <param name="messageSender">Оборачиваемый экземпляр класса рассылки уведомлений.</param>
        /// <param name="fromAddress">Адрес отправителя сообщения.</param>
        /// <param name="toAddresses">Адреса получателей сообщения.</param>
        public WorkflowSampleSystemExceptionMessageSender(
            IConfigurationBLLContext context,
            IMessageSender<Message> messageSender,
            MailAddress fromAddress,
            IEnumerable<string> toAddresses)
            : base(context, messageSender, fromAddress, toAddresses)
        {
        }

        /// <inheritdoc />
        protected override bool SkipSend(Exception exception)
        {
            return false;
        }

        /// <inheritdoc />
        protected override IEnumerable<Type> GetExceptTypes()
        {
            return new[] { typeof(WorkflowSampleSystemException) };
        }
    }
}
