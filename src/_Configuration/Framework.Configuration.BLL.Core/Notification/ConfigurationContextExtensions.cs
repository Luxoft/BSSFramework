using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification;

namespace Framework.Configuration.BLL.Notification
{
    public static class ConfigurationContextExtensions
    {
        public static NotificationMessage GetMessage(this IConfigurationBLLContext context, MessageTemplateNotification messageTemplateNotification)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (messageTemplateNotification == null) throw new ArgumentNullException(nameof(messageTemplateNotification));

            return context.Logics.MessageTemplate.GetByCode(messageTemplateNotification.MessageTemplateCode).Maybe(messageTemplate =>
            {
                var evaluator = context.TemplateEvaluatorFactory.Create<string>(messageTemplate);

                var subject = evaluator.Evaluate(messageTemplate.Subject, messageTemplateNotification.ContextObject);
                var message = evaluator.Evaluate(messageTemplate.Message, messageTemplateNotification.ContextObject);

                return new NotificationMessage { Subject = subject, Body = message, Receivers = messageTemplateNotification.Receivers.ToList() };
            });
        }

        public static NotificationMessage GetMessage<T>(this IConfigurationBLLContext context, string messageTemplateCode, T contextObject, IEnumerable<string> receivers)
        {
            return context.GetMessage(new MessageTemplateNotification(messageTemplateCode, contextObject, typeof(T), receivers, Enumerable.Empty<Attachment>()));
        }
    }
}
