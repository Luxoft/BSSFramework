using System;

namespace Framework.DomainDriven.Generation
{
    public class CheckOutServiceException : Exception
    {
        public CheckOutServiceException(string message)
            : base(message)
        {

        }
    }
}