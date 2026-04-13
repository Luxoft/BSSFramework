using System.Reflection;

using Framework.Core.TypeResolving;
using Framework.ExtendedMetadata;

namespace Framework.Projection._ImplType;

public class GenAssembly(string fullName, string name, ITypeSource typeSource) : Assembly, IWrappingObject
{
    public bool CanWrap => false;

    public override Type[] GetTypes() => [.. typeSource.Types];

    public override string FullName { get; } = fullName;

    public override AssemblyName GetName() => new AssemblyName(name) { Name = name };
}
