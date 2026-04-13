using System.Reflection;

using Framework.BLL.Services;
using Framework.Core.TypeResolving;
using Framework.Database.Metadata;
using Framework.ExtendedMetadata;
using Framework.Projection._ImplType;
using Framework.Projection.Lambda.ProjectionSource._Base;

namespace Framework.Projection.Lambda;

public class DefaultProjectionLambdaEnvironment(
    IProjectionSource projectionSource,
    IMetadataProxyProvider metadataProxyProvider,
    IPropertyPathService propertyPathService,
    string assemblyName,
    string assemblyFullName,
    Type domainObjectBaseType,
    Type persistentDomainObjectBaseType,
    string @namespace,
    bool useDependencySecurity = true) : ProjectionLambdaEnvironment(projectionSource, metadataProxyProvider, propertyPathService)
{
    public override string Namespace { get; } =
        string.IsNullOrWhiteSpace(@namespace) ? throw new ArgumentException("Value cannot be null or whitespace.", nameof(@namespace)) : @namespace;

    public override Assembly Assembly => field ??= new GenAssembly(assemblyFullName, assemblyName, this.ProjectionTypeResolver);

    public override Type DomainObjectBaseType { get; } = domainObjectBaseType;

    public override Type PersistentDomainObjectBaseType { get; } = persistentDomainObjectBaseType;

    /// <inheritdoc />
    public override bool UseDependencySecurity { get; } = useDependencySecurity;
}
