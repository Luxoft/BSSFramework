using System;

namespace Framework.Report
{
    public class EvaluationException : Exception
    {
        public EvaluationException(string message, Exception innerException, string template)
            : base(message, innerException)
        {
            this.Template = template;
        }

        public string Template { get; private set; }
    }
}