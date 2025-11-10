namespace Framework.Core;

public static class MessageSenderExtensions
{
    public static IMessageSender<TNewSource> OverrideInput<TBaseSource, TNewSource>(this IMessageSender<TBaseSource> messageSender, Func<TNewSource, TBaseSource> selector)
    {
        if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new ActionMessageSender<TNewSource>(async (newSource, ct) => await messageSender.SendAsync(selector(newSource), ct));
    }



    public static IMessageSender<TMessage> WithCatchFault<TMessage>(this IMessageSender<TMessage> messageSender)
    {
        if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));

        return new ActionMessageSender<TMessage>(
            async (message, ct) =>
            {
                if (message == null) throw new ArgumentNullException(nameof(message));

                try
                {
                    await messageSender.SendAsync(message, ct);
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

        return new ActionMessageSender<TMessage>(
            async (message, ct) =>
            {
                if (message == null) throw new ArgumentNullException(nameof(message));

                write(message);

                await messageSender.SendAsync(message, ct);
            });
    }

    public static IMessageSender<TMessage> WithTrace<TMessage>(this IMessageSender<TMessage> messageSender)
    {
        if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));

        return messageSender.WithWrite(
            (message) => System.Diagnostics.Trace.Write(
                $"Sending: message: {message}"));
    }



    private class ActionMessageSender<TMessage>(Func<TMessage, CancellationToken, Task> sendAction) : IMessageSender<TMessage>
    {
        public async Task SendAsync(TMessage message, CancellationToken cancellationToken)
        {
            await sendAction(message, cancellationToken);
        }
    }
}
