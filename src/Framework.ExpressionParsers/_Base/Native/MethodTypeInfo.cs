using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.ExpressionParsers;

public class MethodTypeInfo : IEquatable<MethodTypeInfo>
{
    public MethodTypeInfo([NotNull] MethodInfo methodInfo)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

        this.InputTypes = methodInfo.GetParameters().ToArray(p => p.ParameterType);
        this.ReturnType = methodInfo.ReturnType;
    }

    public MethodTypeInfo([NotNull] IEnumerable<Type> inputTypes, [NotNull] Type returnType)
    {
        if (inputTypes == null) throw new ArgumentNullException(nameof(inputTypes));
        if (returnType == null) throw new ArgumentNullException(nameof(returnType));

        this.InputTypes = inputTypes.CheckNotNull().ToArray();
        this.ReturnType = returnType;
    }


    public Type[] InputTypes { get; private set; }

    public Type ReturnType { get; private set; }


    public override bool Equals(object obj)
    {
        return this.Equals(obj as MethodTypeInfo);
    }

    public override int GetHashCode()
    {
        return this.ReturnType.GetHashCode();
    }

    public bool Equals(MethodTypeInfo other)
    {
        return other != null && this.InputTypes.SequenceEqual(other.InputTypes) && this.ReturnType == other.ReturnType;
    }
}
