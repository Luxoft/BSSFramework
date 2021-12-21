using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Exceptions
{
    public class SecurityException : ServiceFacadeException
    {
        public SecurityException(Exception innerException, string format, params object[] args) : base(innerException, format, args)
        {
        }

        public SecurityException(Exception innerException, string message) : base(innerException, message)
        {
        }

        public SecurityException(string format, params object[] args) : base(format, args)
        {
        }

        public SecurityException(string message) : base(message)
        {
        }
    }
}
