using System.Reflection;

using Framework.Core.TypeResolving;
using Framework.ExtendedMetadata;
using Framework.Projection.ImplType;

namespace Framework.Projection.Lambda;

public class ManualProjectionEnvironment(Assembly assembly, Type persistentDomainObjectBaseType, IMetadataProxyProvider metadataProxyProvider) : IProjectionEnvironment
{
    public string Namespace => throw new NotImplementedException("Single namespace not required");

    public Assembly Assembly { get; } = new GeneratedAssembly(
        assembly.FullName!,
        assembly.GetName().Name!,
        new TypeSource([.. assembly.GetTypes().Where(persistentDomainObjectBaseType.IsAssignableFrom)]));

    public bool UseDependencySecurity { get; } = true;

    public IMetadataProxyProvider MetadataProxyProvider { get; } = metadataProxyProvider;
}
