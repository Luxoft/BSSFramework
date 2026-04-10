using System.Reflection;

using Framework.Core.TypeResolving.TypeSource;
using Framework.Database.Metadata;
using Framework.ExtendedMetadata;

namespace Framework.Projection.Lambda;

public class ManualProjectionEnvironment(Assembly assembly, Type persistentDomainObjectBaseType, IMetadataProxyProvider metadataProxyProvider ) : IProjectionEnvironment
{
    public string Namespace => throw new NotImplementedException("Single namespace not required");

    public IAssemblyInfo Assembly { get; } = AssemblyInfo.Create(assembly, persistentDomainObjectBaseType.IsAssignableFrom);

    public bool UseDependencySecurity { get; } = true;

    public IMetadataProxyProvider MetadataProxyProvider  { get; } = metadataProxyProvider ;
}
