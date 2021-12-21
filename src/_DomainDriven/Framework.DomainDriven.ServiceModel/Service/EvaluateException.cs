using System;

namespace Framework.DomainDriven.ServiceModel.Service
{
    public class EvaluateException : Exception
    {
        public EvaluateException(Exception baseException, Exception expandedBaseException)
        {
            this.BaseException = baseException ?? throw new ArgumentNullException(nameof(baseException));
            this.ExpandedBaseException = expandedBaseException ?? throw new ArgumentNullException(nameof(expandedBaseException));
        }

        public Exception BaseException { get; }

        public Exception ExpandedBaseException { get; }

        public override string Message => this.ExpandedBaseException.Message;
    }
}
