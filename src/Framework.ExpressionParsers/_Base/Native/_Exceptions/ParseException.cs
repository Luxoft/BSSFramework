namespace Framework.ExpressionParsers;

public class ParseException(string message, int position) : Exception(message)
{
    public int Position => position;

    public override string ToString() => string.Format(ExpressionParsersResources.ParseExceptionFormat, this.Message, position);
}
