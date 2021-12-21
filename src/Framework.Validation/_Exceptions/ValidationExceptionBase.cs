using System;

namespace Framework.Validation
{
    public class ValidationExceptionBase : Exception
    {
        protected ValidationExceptionBase()
        {
        }

        protected ValidationExceptionBase(string message)
            : base(message)
        {
        }

        protected ValidationExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
