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
        /// <param name="sendMessageMode">Message send mode</param>
        void Send(TMessage message, TransactionMessageMode sendMessageMode = TransactionMessageMode.Auto);
    }
}
