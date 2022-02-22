using System;
using System.Collections.Generic;
using System.Net.Mail;

using Framework.Core;

namespace Framework.Configuration.BLL.Notification
{
    public static partial class MessageSenderExtensions
    {
        public static IMessageSender<Exception> ToExceptionSender(this IMessageSender<Framework.Notification.New.Message> messageSender, IConfigurationBLLContext context, MailAddress sender, IEnumerable<string> receivers)
        {
            if (messageSender == null)
            {
                throw new ArgumentNullException(nameof(messageSender));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (receivers == null)
            {
                throw new ArgumentNullException(nameof(receivers));
            }

            return new ExceptionMessageSender(context, messageSender, sender, receivers);
        }
    }
}
