using System;

using Framework.Core;
using Framework.DomainDriven.Metadata;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

public class DefaultProjectionLambdaEnvironment : ProjectionLambdaEnvironment
{
    public DefaultProjectionLambdaEnvironment(
            [NotNull] IProjectionSource projectionSource,
            [NotNull] string assemblyName,
            [NotNull] string assemblyFullName,
            [NotNull] Type domainObjectBaseType,
            [NotNull] Type persistentDomainObjectBaseType,
            [NotNull] string @namespace,
            bool useDependencySecurity)
            : base(projectionSource)
    {
        if (assemblyName == null) { throw new ArgumentNullException(nameof(assemblyName)); }
        if (assemblyFullName == null) { throw new ArgumentNullException(nameof(assemblyFullName)); }
        if (string.IsNullOrWhiteSpace(@namespace)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(@namespace));


        this.Assembly = LazyInterfaceImplementHelper.CreateProxy<IAssemblyInfo>(() => new AssemblyInfo(assemblyName, assemblyFullName, this.ProjectionTypeResolver));
        this.Namespace = @namespace;
        this.DomainObjectBaseType = domainObjectBaseType ?? throw new ArgumentNullException(nameof(domainObjectBaseType));
        this.PersistentDomainObjectBaseType = persistentDomainObjectBaseType ?? throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));
        this.UseDependencySecurity = useDependencySecurity;
    }

    public override string Namespace { get; }

    public override IAssemblyInfo Assembly { get; }

    public override Type DomainObjectBaseType { get; }

    public override Type PersistentDomainObjectBaseType { get; }

    /// <inheritdoc />
    public override bool UseDependencySecurity { get; }
}
