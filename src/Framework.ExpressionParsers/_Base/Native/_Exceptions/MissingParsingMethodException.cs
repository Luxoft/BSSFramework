using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Framework.Core;

namespace Framework.ExpressionParsers;

public class MissingParsingMethodException : ParseException
{
    public readonly Type ObjType;
    public readonly string MethodName;
    public readonly ReadOnlyCollection<Type> TryArgs;

    public MissingParsingMethodException(string message, int position, Type objType, string methodName, IEnumerable<Type> tryArgs)
            : base(message, position)
    {
        if (objType == null) throw new ArgumentNullException(nameof(objType));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));
        if (tryArgs == null) throw new ArgumentNullException(nameof(tryArgs));


        this.ObjType = objType;
        this.MethodName = methodName;
        this.TryArgs = tryArgs.ToReadOnlyCollection();
    }
}
