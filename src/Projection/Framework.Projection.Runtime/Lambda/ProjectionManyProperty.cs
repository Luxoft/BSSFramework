using System.Linq.Expressions;

namespace Framework.Projection.Lambda;

/// <summary>
/// Projection property for a collection
/// </summary>
/// <typeparam name="TDomainObject">Domain object</typeparam>
/// <typeparam name="TElement">Collection element type</typeparam>
public class ProjectionManyProperty<TDomainObject, TElement> : ProjectionProperty<Expression<Func<TDomainObject, IEnumerable<TElement>>>, TElement>
{
    public ProjectionManyProperty(Expression<Func<TDomainObject, IEnumerable<TElement>>> path, string? name, Func<Projection<TElement>> getPropProjection, bool ignoreSerialization, IEnumerable<Attribute> attributes)
        : base(path, name, getPropProjection, ignoreSerialization, attributes)
    {
        if (getPropProjection is null) throw new ArgumentNullException(nameof(getPropProjection));
    }

    /// <inheritdoc />
    public override Type SourceType { get; } = typeof(TDomainObject);

    /// <inheritdoc />
    public override Type CollectionType { get; } = typeof(IEnumerable<>);
}
