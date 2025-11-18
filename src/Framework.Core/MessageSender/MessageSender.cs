namespace Framework.Core;

public abstract class MessageSender<TMessage> : IMessageSender<TMessage>
{
    public abstract Task SendAsync(TMessage message, CancellationToken cancellationToken);


    public static readonly IMessageSender<TMessage> Empty = new EmptyMessageSender();

    public static readonly IMessageSender<TMessage> Trace = Empty.WithTrace();

    public static readonly IMessageSender<TMessage> NotImplemented = new NotImplementedMessageSender();


    public static IMessageSender<TMessage> Create(TextWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        return Empty.WithWrite((obj) => writer.WriteLine(obj));
    }

    private class EmptyMessageSender : MessageSender<TMessage>
    {
        public EmptyMessageSender()
        {

        }

        public override async Task SendAsync (TMessage message, CancellationToken cancellationToken)
        {

        }
    }

    private class NotImplementedMessageSender : MessageSender<TMessage>
    {
        public NotImplementedMessageSender()
        {

        }

        public override async Task SendAsync(TMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
