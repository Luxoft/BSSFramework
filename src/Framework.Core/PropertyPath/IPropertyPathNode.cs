using System;
using System.Collections.Generic;

namespace Framework.Core;

public interface IPropertyPathNode<out TSource>
{
    IPropertyPathNode<TSource> Select<TProperty>(Func<TSource, TProperty> path);

    IPropertyPathNode<TProperty> SelectMany<TProperty>(Func<TSource, IEnumerable<TProperty>> path);

    IPropertyPathNode<TProperty> SelectNested<TProperty>(Func<TSource, TProperty> path);
}
