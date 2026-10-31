using System.Linq.Expressions;
using System.Reflection;

using Anch.Core;

using Framework.Core;

namespace Framework.Projection.Lambda;

/// <summary>
/// Projection property
/// </summary>
public abstract class ProjectionProperty<TExpression, TElement> : IProjectionProperty
    where TExpression : LambdaExpression
{
    private readonly Lazy<Projection<TElement>>? lazyElementProjection;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="path">Path to the property</param>
    /// <param name="name">Property name</param>
    /// <param name="getPropProjection">Property projection type</param>
    /// <param name="ignoreSerialization">Ignore serialization</param>
    /// <param name="attributes">Additional property attributes</param>
    protected ProjectionProperty(TExpression path, string? name, Func<Projection<TElement>>? getPropProjection, bool ignoreSerialization, IEnumerable<Attribute> attributes)
    {
        this.Expression = path ?? throw new ArgumentNullException(nameof(path));
        this.IgnoreSerialization = ignoreSerialization;
        this.Path = this.Expression.ToPropertyPath();
        this.Name = name ?? path.ToPath().Replace(".", "");
        this.IsNullable = typeof(TElement).IsValueType && this.Path.HasReferenceResult();

        this.ElementType = typeof(TElement).GetNullableElementTypeOrSelf();

        this.lazyElementProjection = getPropProjection.Maybe(LazyHelper.Create);

        this.Attributes = (attributes ?? throw new ArgumentNullException(nameof(attributes))).ToReadOnlyCollection();
    }

    /// <inheritdoc />
    public ProjectionPropertyRole Role { get; } = ProjectionPropertyRole.Default;

    /// <summary>
    /// Expression path to the property
    /// </summary>
    public TExpression Expression { get; }

    /// <inheritdoc />
    public PropertyPath Path { get; }

    /// <inheritdoc />
    public string Name { get; }

    public IProjection? ElementProjection => this.lazyElementProjection.Maybe(v => v.Value);

    /// <inheritdoc />
    public abstract Type SourceType { get; }

    /// <summary>
    /// Projection onto the property type
    /// </summary>
    public Type ElementType { get; }

    /// <summary>
    /// Collection type
    /// </summary>
    public abstract Type? CollectionType { get; }

    /// <summary>
    /// The property is a collection
    /// </summary>
    public bool IsCollection => this.CollectionType is not null;

    /// <summary>
    /// The property has a Nullable type
    /// </summary>
    public bool IsNullable { get; }

    /// <inheritdoc />
    public IReadOnlyList<Attribute> Attributes { get; }

    /// <inheritdoc />
    public bool IgnoreSerialization { get; }

    LambdaExpression IProjectionProperty.Expression => this.Expression;

    TypeReferenceBase.BuildTypeReference IProjectionProperty.Type => new(this.ElementType, this.CollectionType, this.IsNullable, this.ElementProjection);

    PropertyInfo? IProjectionProperty.VirtualExplicitInterfaceProperty { get; } = null;
}
