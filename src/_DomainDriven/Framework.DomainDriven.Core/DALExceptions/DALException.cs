using System;

namespace Framework.DomainDriven
{
    public abstract class DALException : Exception
    {
        protected DALException(string message)
            : base(message)
        {

        }
    }
}