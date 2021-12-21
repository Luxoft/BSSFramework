using System;

namespace Framework.ExpressionParsers
{
    public class ParseException : Exception
    {
        int position;

        public ParseException(string message, int position)
            : base(message)
        {
            this.position = position;
        }

        public int Position
        {
            get { return this.position; }
        }

        public override string ToString()
        {
            return string.Format(ExpressionParsersResources.ParseExceptionFormat, this.Message, this.position);
        }

    }
}