using System.Reflection;

using Framework.Core.TypeResolving.TypeSource;
using Framework.Database.Metadata;
using Framework.Projection.ExtendedMetadata;

namespace Framework.Projection.Lambda;

public class ManualProjectionEnvironment(Assembly assembly, Type persistentDomainObjectBaseType, IDomainTypeRootExtendedMetadata extendedMetadata) : IProjectionEnvironment
{
    public string Namespace => throw new NotImplementedException("Single namespace not required");

    public IAssemblyInfo Assembly { get; } = AssemblyInfo.Create(assembly, persistentDomainObjectBaseType.IsAssignableFrom);

    public bool UseDependencySecurity { get; } = true;

    public IDomainTypeRootExtendedMetadata ExtendedMetadata { get; } = extendedMetadata;
}
