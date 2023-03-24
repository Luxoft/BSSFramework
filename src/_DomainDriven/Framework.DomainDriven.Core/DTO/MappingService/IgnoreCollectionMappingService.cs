using System;
using System.Collections.Generic;

namespace Framework.DomainDriven;

public class IgnoreCollectionMappingService<TSource, TTarget> : ICollectionMappingService<TSource, TTarget>
{
    private IgnoreCollectionMappingService()
    {

    }

    public void Map(IEnumerable<TSource> source, IEnumerable<TTarget> target)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));
    }

    public static readonly IgnoreCollectionMappingService<TSource, TTarget> Vaue = new IgnoreCollectionMappingService<TSource, TTarget>();
}
