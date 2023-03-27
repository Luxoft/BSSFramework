using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection;

public abstract class ProjectionEnvironmentBase : IProjectionEnvironment
{
    private readonly Lazy<PropertyInfo> lazyIdentityProperty;

    protected ProjectionEnvironmentBase()
    {
        this.lazyIdentityProperty = LazyHelper.Create(this.GetIdentityProperty);
    }


    public abstract string Namespace { get; }

    public abstract Type DomainObjectBaseType { get; }

    public abstract Type PersistentDomainObjectBaseType { get; }

    public PropertyInfo IdentityProperty => this.lazyIdentityProperty.Value;

    public abstract IAssemblyInfo Assembly { get; }

    /// <inheritdoc />
    public abstract bool UseDependencySecurity { get; }

    protected virtual PropertyInfo GetIdentityProperty()
    {
        return this.PersistentDomainObjectBaseType.GetProperty("Id", true);
    }

    /// <summary>
    /// Проверка на то, что свойство является Id-ом
    /// </summary>
    /// <param name="property">свойство</param>
    /// <returns></returns>
    public virtual bool IsIdentityProperty([NotNull] PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.IdentityProperty.IsAssignableFrom(property);
    }

    /// <summary>
    /// Проверка на то, что тип является персистентным
    /// </summary>
    /// <param name="type">проверяемый тип</param>
    /// <returns></returns>
    public bool IsPersistent([NotNull] Type type)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }

        return this.PersistentDomainObjectBaseType.IsAssignableFrom(type);
    }
}
