using System;

namespace Framework.Workflow.BLL
{
    public class ValidateParameterValueException : Exception
    {
        public ValidateParameterValueException(string message)
            : base(message)
        {

        }

        public ValidateParameterValueException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}