using System.Collections.ObjectModel;

using CommonFramework;

namespace Framework.ExpressionParsers.Native.Exceptions;

public class MissingParsingMethodException(string message, int position, Type objType, string methodName, IEnumerable<Type> tryArgs)
    : ParseException(message, position)
{
    public readonly Type ObjType = objType;

    public readonly string MethodName = methodName;

    public readonly ReadOnlyCollection<Type> TryArgs = tryArgs.ToReadOnlyCollection();
}
