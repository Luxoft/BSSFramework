// ReSharper disable once CheckNamespace
namespace Framework.Core;

public interface IMessageSenderContainer<in TMessage>
{
    IMessageSender<TMessage> MessageSender { get; }
}
