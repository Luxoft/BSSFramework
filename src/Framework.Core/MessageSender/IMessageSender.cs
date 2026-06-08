// ReSharper disable once CheckNamespace
namespace Framework.Core;

/// <summary>
/// Represents message sender interface
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IMessageSender<in TMessage>
{
    Task SendAsync(TMessage message, CancellationToken cancellationToken = default);
}
