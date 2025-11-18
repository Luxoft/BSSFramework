namespace Framework.Core;

/// <summary>
/// Represents message sender interface
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IMessageSender<in TMessage>
{
    /// <summary>
    /// Sends message
    /// </summary>
    /// <param name="message">Message to send</param>
    void Send(TMessage message) => this.SendAsync(message, CancellationToken.None).GetAwaiter().GetResult();

    Task SendAsync(TMessage message, CancellationToken cancellationToken = default);
}
