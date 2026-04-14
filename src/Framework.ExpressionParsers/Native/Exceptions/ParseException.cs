using Framework.ExpressionParsers.CSharp;

namespace Framework.ExpressionParsers.Native.Exceptions;

public class ParseException(string message, int position) : Exception(message)
{
    public int Position => position;

    public override string ToString() => string.Format(ExpressionParsersResources.ParseExceptionFormat, this.Message, position);
}
