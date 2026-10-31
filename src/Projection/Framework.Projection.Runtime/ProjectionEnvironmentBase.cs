using System.Reflection;

using Anch.Core;

using Framework.Core;
using Framework.ExtendedMetadata;

namespace Framework.Projection;

public abstract class ProjectionEnvironmentBase : IProjectionEnvironment
{
    private readonly Lazy<PropertyInfo> lazyIdentityProperty;

    protected ProjectionEnvironmentBase(IMetadataProxyProvider metadataProxyProvider)
    {
        this.MetadataProxyProvider = metadataProxyProvider;

        this.lazyIdentityProperty = LazyHelper.Create(this.GetIdentityProperty);
    }

    public IMetadataProxyProvider MetadataProxyProvider { get; }

    public abstract string Namespace { get; }

    public abstract Type DomainObjectBaseType { get; }

    public abstract Type PersistentDomainObjectBaseType { get; }

    public PropertyInfo IdentityProperty => this.lazyIdentityProperty.Value;

    public abstract Assembly Assembly { get; }

    /// <inheritdoc />
    public abstract bool UseDependencySecurity { get; }

    protected virtual PropertyInfo GetIdentityProperty() => this.PersistentDomainObjectBaseType.GetProperty("Id", true)!;

    /// <summary>
    /// Check that the property is an Id
    /// </summary>
    /// <param name="property">property</param>
    /// <returns></returns>
    public virtual bool IsIdentityProperty(PropertyInfo property)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));

        return this.IdentityProperty.IsAssignableFrom(property);
    }

    /// <summary>
    /// Check that the type is persistent
    /// </summary>
    /// <param name="type">type to check</param>
    /// <returns></returns>
    public bool IsPersistent(Type type)
    {
        if (type is null) { throw new ArgumentNullException(nameof(type)); }

        return this.PersistentDomainObjectBaseType.IsAssignableFrom(type);
    }
}
