using System;
using System.Runtime.Serialization;

namespace WorkflowSampleSystem.BLL
{
    /// <summary>
    ///     Исключение WorkflowSampleSystem.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class WorkflowSampleSystemException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowSampleSystemException" /> class.
        /// </summary>
        public WorkflowSampleSystemException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowSampleSystemException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WorkflowSampleSystemException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowSampleSystemException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public WorkflowSampleSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowSampleSystemException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        protected WorkflowSampleSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
