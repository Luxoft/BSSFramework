using System.Reflection;

using Framework.Core.TypeResolving;

namespace Framework.Projection._ImplType;

public class GenAssembly(string fullName, string name, ITypeSource typeSource) : Assembly
{
    public override Type[] GetTypes() => [.. typeSource.Types];

    public override string FullName { get; } = fullName;
}
