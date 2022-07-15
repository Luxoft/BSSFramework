namespace Framework.Core
{
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
        void Send(TMessage message);
    }
}
