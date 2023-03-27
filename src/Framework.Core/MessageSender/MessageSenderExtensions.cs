namespace Framework.Core;

public static class MessageSenderExtensions
{
    public static IMessageSender<TNewSource> OverrideInput<TBaseSource, TNewSource>(this IMessageSender<TBaseSource> messageSender, Func<TNewSource, TBaseSource> selector)
    {
        if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new ActionMessageSender<TNewSource>((newSource) => messageSender.Send(selector(newSource)));
    }



    public static IMessageSender<TMessage> WithCatchFault<TMessage>(this IMessageSender<TMessage> messageSender)
    {
        if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));

        return new ActionMessageSender<TMessage>((message) =>
                                                 {
                                                     if (message == null) throw new ArgumentNullException(nameof(message));

                                                     try
                                                     {
                                                         messageSender.Send(message);
                                                     }
                                                     catch
                                                     {
                                                         // ignored
                                                     }
                                                 });
    }

    public static IMessageSender<TMessage> WithWrite<TMessage>(this IMessageSender<TMessage> messageSender, Action<TMessage> write)
    {
        if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));
        if (write == null) throw new ArgumentNullException(nameof(write));

        return new ActionMessageSender<TMessage>((message) =>
                                                 {
                                                     if (message == null) throw new ArgumentNullException(nameof(message));

                                                     write(message);

                                                     messageSender.Send(message);
                                                 });
    }

    public static IMessageSender<TMessage> WithTrace<TMessage>(this IMessageSender<TMessage> messageSender)
    {
        if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));

        return messageSender.WithWrite((message) => System.Diagnostics.Trace.Write(
                                                                                   $"Sending: message: {message}"));
    }



    private class ActionMessageSender<TMessage> : IMessageSender<TMessage>
    {
        private readonly Action<TMessage> _sendAction;


        public ActionMessageSender(Action<TMessage> sendAction)
        {
            if (sendAction == null) throw new ArgumentNullException(nameof(sendAction));

            this._sendAction = sendAction;
        }


        public void Send(TMessage message)
        {
            this._sendAction(message);
        }
    }
}
