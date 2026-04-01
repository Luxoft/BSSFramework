using System.Reflection;
using Framework.Core;

namespace Framework.ExpressionParsers.Native;

public class MethodTypeInfo : IEquatable<MethodTypeInfo>
{
    public MethodTypeInfo(MethodInfo methodInfo)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

        this.InputTypes = methodInfo.GetParameters().ToArray(p => p.ParameterType);
        this.ReturnType = methodInfo.ReturnType;
    }

    public MethodTypeInfo(IEnumerable<Type> inputTypes, Type returnType)
    {
        if (inputTypes == null) throw new ArgumentNullException(nameof(inputTypes));
        if (returnType == null) throw new ArgumentNullException(nameof(returnType));

        this.InputTypes = inputTypes.ToArray();
        this.ReturnType = returnType;
    }


    public Type[] InputTypes { get; }

    public Type ReturnType { get; }


    public override bool Equals(object obj) => this.Equals(obj as MethodTypeInfo);

    public override int GetHashCode() => this.ReturnType.GetHashCode();

    public bool Equals(MethodTypeInfo other) => other != null && this.InputTypes.SequenceEqual(other.InputTypes) && this.ReturnType == other.ReturnType;
}
