using System;
using Framework.Core;

namespace Framework.Notification
{
    public interface IExceptionSenderContainer
    {
        IMessageSender<Exception> ExceptionSender { get; }
    }
}