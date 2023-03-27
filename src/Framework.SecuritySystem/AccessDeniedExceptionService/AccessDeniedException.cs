using System;

namespace Framework.SecuritySystem.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message)
            : base(message)
        {

        }
    }
}
