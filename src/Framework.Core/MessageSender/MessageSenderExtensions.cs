using System;
using Framework.Core;

namespace Framework.Core
{
    public static class MessageSenderExtensions
    {
        public static IMessageSender<TNewSource> OverrideInput<TBaseSource, TNewSource>(this IMessageSender<TBaseSource> messageSender, Func<TNewSource, TBaseSource> selector)
        {
            if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new ActionMessageSender<TNewSource>((newSource, transactionMode)=> messageSender.Send(selector(newSource), transactionMode));
        }



        public static IMessageSender<TMessage> WithCatchFault<TMessage>(this IMessageSender<TMessage> messageSender)
        {
            if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));

            return new ActionMessageSender<TMessage>((message, transactionMessageMode) =>
            {
                if (message == null) throw new ArgumentNullException(nameof(message));

                try
                {
                    messageSender.Send(message, transactionMessageMode);
                }
                catch
                {
                    // ignored
                }
            });
        }

        public static IMessageSender<TMessage> WithWrite<TMessage>(this IMessageSender<TMessage> messageSender, Action<TMessage, TransactionMessageMode> write)
        {
            if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));
            if (write == null) throw new ArgumentNullException(nameof(write));

            return new ActionMessageSender<TMessage>((message, transactionMessageMode) =>
            {
                if (message == null) throw new ArgumentNullException(nameof(message));

                write(message, transactionMessageMode);

                messageSender.Send(message, transactionMessageMode);
            });
        }

        public static IMessageSender<TMessage> WithTrace<TMessage>(this IMessageSender<TMessage> messageSender)
        {
            if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));

            return messageSender.WithWrite((message, transactionMessageMode) => System.Diagnostics.Trace.Write(
                $"Sending: mode:{transactionMessageMode} message: {message}"));
        }



        private class ActionMessageSender<TMessage> : IMessageSender<TMessage>
        {
            private readonly Action<TMessage, TransactionMessageMode> _sendAction;


            public ActionMessageSender(Action<TMessage, TransactionMessageMode> sendAction)
            {
                if (sendAction == null) throw new ArgumentNullException(nameof(sendAction));

                this._sendAction = sendAction;
            }


            public void Send(TMessage message, TransactionMessageMode transactionMessageMode)
            {
                this._sendAction(message, transactionMessageMode);
            }
        }
    }
}