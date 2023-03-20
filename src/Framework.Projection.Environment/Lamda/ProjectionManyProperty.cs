using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

/// <summary>
/// Проекционное свойство на колеекцию
/// </summary>
/// <typeparam name="TDomainObject">Доменный объект</typeparam>
/// <typeparam name="TElement">Тип элемента коллекции</typeparam>
public class ProjectionManyProperty<TDomainObject, TElement> : ProjectionProperty<Expression<Func<TDomainObject, IEnumerable<TElement>>>, TElement>
{
    public ProjectionManyProperty(Expression<Func<TDomainObject, IEnumerable<TElement>>> path, string name, [NotNull] Func<Projection<TElement>> getPropProjection, bool ignoreSerialization, IEnumerable<Attribute> attributes)
            : base(path, name, getPropProjection, ignoreSerialization, attributes)
    {
        if (getPropProjection == null) throw new ArgumentNullException(nameof(getPropProjection));
    }

    /// <inheritdoc />
    public override Type SourceType { get; } = typeof(TDomainObject);

    /// <inheritdoc />
    public override Type CollectionType { get; } = typeof(IEnumerable<>);
}
