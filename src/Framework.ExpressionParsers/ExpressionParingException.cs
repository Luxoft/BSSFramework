namespace Framework.ExpressionParsers;

public class ExpressionParingException(string message, Exception innerException) : Exception(message, innerException);
