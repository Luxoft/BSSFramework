using System.Linq.Expressions;

namespace Framework.Projection.Lambda;

/// <summary>
/// Проекционное свойство на одиночный элемент
/// </summary>
/// <typeparam name="TDomainObject">Доменный объект</typeparam>
/// <typeparam name="TElement">Тип элемента</typeparam>
public class ProjectionSingleProperty<TDomainObject, TElement> : ProjectionProperty<Expression<Func<TDomainObject, TElement>>, TElement>
{
    public ProjectionSingleProperty(Expression<Func<TDomainObject, TElement>> path, string name, Func<Projection<TElement>> getPropProjection, bool ignoreSerialization, IEnumerable<Attribute> attributes)
            : base(path, name, getPropProjection, ignoreSerialization, attributes)
    {
    }

    /// <inheritdoc />
    public override Type SourceType { get; } = typeof(TDomainObject);

    /// <inheritdoc />
    public override Type CollectionType { get; } = null;
}
