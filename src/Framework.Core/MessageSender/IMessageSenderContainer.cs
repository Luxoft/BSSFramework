namespace Framework.Core.MessageSender;

public interface IMessageSenderContainer<in TMessage>
{
    IMessageSender<TMessage> MessageSender { get; }
}
