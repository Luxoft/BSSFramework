using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Projection.Lambda;

/// <summary>
/// Projection metadata
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public class Projection<TDomainObject> : IProjection
{
    private readonly IReadOnlyList<IProjectionCustomProperty> customProperties = new List<IProjectionCustomProperty>();

    private readonly IReadOnlyList<Attribute> projectionAttributes = new List<Attribute>();

    private readonly IReadOnlyList<ProjectionFilterAttribute> filterAttributes = new List<ProjectionFilterAttribute>();

    private readonly IReadOnlyList<IProjectionProperty> properties = new List<IProjectionProperty>();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">Projection name</param>
    /// <param name="bllView">Generation of BLL and facade methods</param>
    public Projection(string name, bool bllView = false)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

        this.Name = name;
        this.BLLView = bllView;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="getName">Getting the projection name</param>
    /// <param name="bllView">Generation of BLL and facade methods</param>
    public Projection(Expression<Func<Projection<TDomainObject>>> getName, bool bllView = false)
        : this(getName.GetInstanceMemberName(), bllView)
    {
    }

    internal Projection(
        string name,
        bool bllView,
        IEnumerable<IProjectionProperty> properties,
        IEnumerable<ProjectionFilterAttribute> filterAttributes,
        IEnumerable<IProjectionCustomProperty> customProperties,
        IEnumerable<Attribute> attributes)
        : this(name, bllView)
    {
        this.properties = (properties ?? throw new ArgumentNullException(nameof(properties))).ToList();
        this.filterAttributes = (filterAttributes ?? throw new ArgumentNullException(nameof(filterAttributes))).ToList();
        this.customProperties = (customProperties ?? throw new ArgumentNullException(nameof(customProperties))).ToList();
        this.projectionAttributes = (attributes ?? throw new ArgumentNullException(nameof(attributes))).ToList();
    }

    /// <inheritdoc />
    public Type SourceType { get; } = typeof(TDomainObject);

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool BLLView { get; }

    /// <inheritdoc />
    public ProjectionRole Role { get; } = ProjectionRole.Default;

    /// <summary>
    /// Add a property to the projection
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="path">Path to the property</param>
    /// <param name="name">Property name</param>
    /// <param name="ignoreSerialization">Disable serialization in DTO</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> Property<TProperty>(
        Expression<Func<TDomainObject, TProperty>> path,
        string? name = null,
        bool ignoreSerialization = false,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        return this.AddProperty(new ProjectionSingleProperty<TDomainObject, TProperty>(path, name, null, ignoreSerialization, propertyAttributes ?? []));
    }

    /// <summary>
    /// Add a property to the projection
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="path">Path to the property</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <param name="name">Property name</param>
    /// <param name="ignoreSerialization">Disable serialization in DTO</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> Property<TProperty>(
        Expression<Func<TDomainObject, TProperty>> path,
        Func<Projection<TProperty>> getPropProjection,
        string? name = null,
        bool ignoreSerialization = false,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));
        if (getPropProjection is null) throw new ArgumentNullException(nameof(getPropProjection));

        return this.AddProperty(new ProjectionSingleProperty<TDomainObject, TProperty>(path, name, getPropProjection, ignoreSerialization, propertyAttributes ?? []));
    }

    /// <summary>
    /// Add a collection property to the projection
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="path">Path to the property</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <param name="name">Property name</param>
    /// <param name="ignoreSerialization">Disable serialization in DTO</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> Property<TProperty>(
        Expression<Func<TDomainObject, IEnumerable<TProperty>>> path,
        Func<Projection<TProperty>> getPropProjection,
        string? name = null,
        bool ignoreSerialization = false,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));
        if (getPropProjection is null) throw new ArgumentNullException(nameof(getPropProjection));

        return this.AddProperty(new ProjectionManyProperty<TDomainObject, TProperty>(path, name, getPropProjection, ignoreSerialization, propertyAttributes ?? []));
    }

    internal Projection<TDomainObject> AddProperty(IProjectionProperty newProp)
    {
        if (newProp is null) throw new ArgumentNullException(nameof(newProp));

        return new Projection<TDomainObject>(this.Name, this.BLLView, this.properties.Concat([newProp]), this.filterAttributes, this.customProperties, this.projectionAttributes);
    }

    /// <summary>
    /// Add a calculated property to the projection
    /// </summary>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="name">Property name</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <param name="fetchs">Properties to fetch</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(
        string name,
        Func<Projection<TProperty>>? getPropProjection = null,
        string[]? fetchs = null,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return this.CustomProperty(name, false, getPropProjection, fetchs, propertyAttributes);
    }

    /// <summary>
    /// Add a calculated property to the projection
    /// </summary>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="name">Property name</param>
    /// <param name="writable">Property is writable</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <param name="fetchs">Properties to fetch</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(
        string name,
        bool writable,
        Func<Projection<TProperty>>? getPropProjection = null,
        string[]? fetchs = null,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return new Projection<TDomainObject>(
            this.Name,
            this.BLLView,
            this.properties,
            this.filterAttributes,
            this.customProperties.Concat(
            [
                new ProjectionCustomProperty<TProperty>(name, writable, getPropProjection, null, fetchs, propertyAttributes)
            ]),
            this.projectionAttributes);
    }


    /// <summary>
    /// Add a calculated property to the projection
    /// </summary>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="path">Path to the property</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <param name="fetchs">Properties to fetch</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(
        Expression<Func<TDomainObject, TProperty>> path,
        Func<Projection<TProperty>>? getPropProjection = null,
        string[]? fetchs = null,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        return this.CustomProperty(
            path.ToPath().Replace(".", string.Empty),
            getPropProjection,
            fetchs,
            propertyAttributes);
    }

    /// <summary>
    /// Add a calculated property to the projection
    /// </summary>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="path">Path to the property</param>
    /// <param name="writable">Property is writable</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <param name="fetchs">Properties to fetch</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(
        Expression<Func<TDomainObject, TProperty>> path,
        bool writable,
        Func<Projection<TProperty>>? getPropProjection = null,
        string[]? fetchs = null,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        return this.CustomProperty(
            path.ToPath().Replace(".", string.Empty),
            writable,
            getPropProjection,
            fetchs,
            propertyAttributes);
    }

    /// <summary>
    /// Add a collection calculated property to the projection
    /// </summary>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="name">Property name</param>
    /// <param name="writable">Property is writable</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <param name="collectionType">Projection collection type</param>
    /// <param name="fetchs">Properties to fetch</param>
    /// <param name="propertyAttributes">Additional property attributes</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomManyProperty<TProperty>(
        string name,
        bool writable,
        Func<Projection<TProperty>>? getPropProjection,
        Type collectionType,
        string[]? fetchs = null,
        IEnumerable<Attribute>? propertyAttributes = null)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return new Projection<TDomainObject>(
            this.Name,
            this.BLLView,
            this.properties,
            this.filterAttributes,
            this.customProperties.Concat(
            [
                new ProjectionCustomProperty<TProperty>(name, writable, getPropProjection, collectionType, fetchs, propertyAttributes)
            ]),
            this.projectionAttributes);
    }

    /// <summary>
    /// Add a collection calculated property to the projection
    /// </summary>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="name">Property name</param>
    /// <param name="writable">Property is writable</param>
    /// <param name="getPropProjection">Projection onto the property type</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomManyProperty<TProperty>(string name, bool writable, Func<Projection<TProperty>>? getPropProjection)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return this.CustomManyProperty(name, writable, getPropProjection, typeof(IEnumerable<>));
    }

    /// <summary>
    /// Add a collection calculated property to the projection
    /// </summary>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="name">Property name</param>
    /// <param name="writable">Property is writable</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomManyProperty<TProperty>(string name, bool writable)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return this.CustomManyProperty<TProperty>(name, writable, null);
    }

    /// <summary>
    /// Copy of the projection with a new name
    /// </summary>
    /// <param name="name">Projection name</param>
    /// <param name="bllView">Generation of BLL and facade methods</param>
    /// <returns></returns>
    public Projection<TDomainObject> OverrideHeader(string name, bool? bllView = null)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));

        return new Projection<TDomainObject>(
            name,
            bllView ?? this.BLLView,
            this.properties,
            this.filterAttributes,
            this.customProperties,
            this.projectionAttributes);
    }

    /// <summary>
    /// Copy of the projection with a new name
    /// </summary>
    /// <param name="getName">Getting the projection name</param>
    /// <param name="bllView">Generation of BLL and facade methods</param>
    /// <returns></returns>
    public Projection<TDomainObject> OverrideHeader(Expression<Func<Projection<TDomainObject>>> getName, bool? bllView = null) =>
        this.OverrideHeader(getName.GetInstanceMemberName(), bllView);

    /// <summary>
    /// Add all properties from another projection except headers
    /// </summary>
    /// <param name="propertiesSource"></param>
    /// <returns></returns>
    public Projection<TDomainObject> AddSource(Projection<TDomainObject> propertiesSource) =>
        new(
            this.Name,
            this.BLLView,
            this.properties.Concat(propertiesSource.properties),
            this.filterAttributes.Concat(propertiesSource.filterAttributes),
            this.customProperties.Concat(propertiesSource.customProperties),
            this.projectionAttributes.Concat(propertiesSource.projectionAttributes));

    /// <summary>
    /// Add a filter by which projection objects are fetched
    /// </summary>
    /// <param name="filterType">Filter type</param>
    /// <param name="target">Filter applicability</param>
    /// <returns></returns>
    public Projection<TDomainObject> Filter(Type filterType, ProjectionFilterTargets target = ProjectionFilterTargets.OData)
    {
        if (filterType is null) throw new ArgumentNullException(nameof(filterType));

        return new Projection<TDomainObject>(
            this.Name,
            this.BLLView,
            this.properties,
            this.filterAttributes.Concat([new ProjectionFilterAttribute(filterType, target)]),
            this.customProperties,
            this.projectionAttributes);
    }

    /// <summary>
    /// Add a filter by which projection objects are fetched
    /// </summary>
    /// <typeparam name="TFilter">Filter type</typeparam>
    /// <param name="target">Filter applicability</param>
    /// <returns></returns>
    public Projection<TDomainObject> Filter<TFilter>(ProjectionFilterTargets target = ProjectionFilterTargets.OData) => this.Filter(typeof(TFilter), target);

    /// <summary>
    /// Add a custom attribute
    /// </summary>
    /// <param name="attribute">Custom attribute</param>
    /// <returns></returns>
    public Projection<TDomainObject> Attribute(Attribute attribute)
    {
        if (attribute is null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        return new Projection<TDomainObject>(this.Name, this.BLLView, this.properties, this.filterAttributes, this.customProperties, this.projectionAttributes.Concat([attribute]));
    }


    IReadOnlyList<IProjectionProperty> IProjection.Properties => this.properties;

    IReadOnlyList<IProjectionCustomProperty> IProjection.CustomProperties => this.customProperties;

    IReadOnlyList<Attribute> IProjectionAttributeProvider.Attributes => this.projectionAttributes;

    IReadOnlyList<ProjectionFilterAttribute> IProjection.FilterAttributes => this.filterAttributes;

    bool IProjection.IgnoreIdSerialization { get; } = false;
}
