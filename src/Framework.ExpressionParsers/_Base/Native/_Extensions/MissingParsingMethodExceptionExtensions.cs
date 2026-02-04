namespace Framework.ExpressionParsers;

public static class MissingParsingMethodExceptionExtensions
{
    internal static Exception ToMissingParsingMethodException(this Type obj, string message, int position, string methodName, IEnumerable<Type> tryArgs)
    {
        return new MissingParsingMethodException(message, position, obj, methodName, tryArgs);
    }


    public static Type? GetArgumentType<TDelegate, TArg>(this INativeExpressionParser nativeExpression, string expression)
    {
        if (nativeExpression == null) throw new ArgumentNullException(nameof(nativeExpression));

        var parser = CSharpNativeExpressionParser.Default;

        try
        {
            parser.Parse<TDelegate>(expression);
            return null;
        }
        catch (MissingParsingMethodException ex)
        {
            var methods = ex.ObjType.GetMethods().Where(m => m.Name == ex.MethodName).ToList();

            var request = from method in methods

                          let parameters = method.GetParameters()

                          where parameters.Length == ex.TryArgs.Count

                          let argIndex = ex.TryArgs.IndexOf(typeof (TArg))

                          where argIndex != -1

                          select parameters[argIndex].ParameterType;

            return request.SingleOrDefault();
        }
    }
}
