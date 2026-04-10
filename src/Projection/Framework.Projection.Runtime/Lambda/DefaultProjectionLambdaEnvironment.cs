using Framework.BLL.Services;
using Framework.Core;
using Framework.Core.TypeResolving.TypeSource;
using Framework.Database.Metadata;
using Framework.ExtendedMetadata;
using Framework.Projection.Lambda.ProjectionSource._Base;

namespace Framework.Projection.Lambda;

public class DefaultProjectionLambdaEnvironment : ProjectionLambdaEnvironment
{
    public DefaultProjectionLambdaEnvironment(
        IProjectionSource projectionSource,
        IMetadataProxyProvider metadataProxyProvider,
        IPropertyPathService propertyPathService,
        string assemblyName,
        string assemblyFullName,
        Type domainObjectBaseType,
        Type persistentDomainObjectBaseType,
        string @namespace,
        bool useDependencySecurity = true)
        : base(projectionSource, metadataProxyProvider, propertyPathService)
    {
        if (assemblyName == null)
        {
            throw new ArgumentNullException(nameof(assemblyName));
        }

        if (assemblyFullName == null)
        {
            throw new ArgumentNullException(nameof(assemblyFullName));
        }

        if (string.IsNullOrWhiteSpace(@namespace)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(@namespace));


        this.Assembly = LazyInterfaceImplementHelper.CreateProxy<IAssemblyInfo>(
            () => new AssemblyInfo(assemblyName, assemblyFullName, this.ProjectionTypeResolver));
        this.Namespace = @namespace;
        this.DomainObjectBaseType = domainObjectBaseType ?? throw new ArgumentNullException(nameof(domainObjectBaseType));
        this.PersistentDomainObjectBaseType =
            persistentDomainObjectBaseType ?? throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));
        this.UseDependencySecurity = useDependencySecurity;
    }

    public override string Namespace { get; }

    public override IAssemblyInfo Assembly { get; }

    public override Type DomainObjectBaseType { get; }

    public override Type PersistentDomainObjectBaseType { get; }

    /// <inheritdoc />
    public override bool UseDependencySecurity { get; }
}
