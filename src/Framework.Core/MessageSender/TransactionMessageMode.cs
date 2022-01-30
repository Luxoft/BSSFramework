using System;

namespace Framework.Core
{
    /// <summary>
    /// Константы, определяющие режим отправки сообщений
    /// </summary>
    public enum TransactionMessageMode
    {
        /// <summary>
        /// Use internal transaction mode
        /// </summary>
        InternalTransaction,

        /// <summary>
        /// Use transactionScope
        /// </summary>
        [Obsolete("Please do not use TransactionScope as legacy API")]
        DTSTransaction,

        /// <summary>
        /// If exists transaction scope use DTSTransaction mode else InternalTransaction
        /// </summary>
        Auto,
    }
}
