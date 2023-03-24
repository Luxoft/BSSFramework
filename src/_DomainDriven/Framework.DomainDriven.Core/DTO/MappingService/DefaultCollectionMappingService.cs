using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven;

public class DefaultCollectionMappingService<TSource, TTarget, TIdent> : ICollectionMappingService<TSource, TTarget>

        where TSource : IIdentityObject<TIdent>

        where TTarget : class, IIdentityObject<TIdent>
{
    private readonly Func<TSource, TTarget> _createAndMapDetail;
    private readonly Action<TTarget> _removeDetail;


    public DefaultCollectionMappingService(Func<TSource, TTarget> createAndMapDetail, Action<TTarget> removeDetail)
    {
        if (createAndMapDetail == null) throw new ArgumentNullException(nameof(createAndMapDetail));
        if (removeDetail == null) throw new ArgumentNullException(nameof(removeDetail));


        this._createAndMapDetail = createAndMapDetail;
        this._removeDetail = removeDetail;
    }


    protected virtual TTarget AddDetail(TSource source)
    {
        return this._createAndMapDetail(source);
    }

    protected virtual void RemoveDetail(TTarget target)
    {
        this._removeDetail(target);
    }

    protected virtual TIdent GetSourceId(TSource source)
    {
        return source.Id;
    }

    protected virtual TIdent GetTargetId(TTarget target)
    {
        return target.Id;
    }


    public void Map(IEnumerable<TSource> source, IEnumerable<TTarget> target)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));

        target.Merge(source, this.GetSourceId,
                     this.GetTargetId,
                     this.AddDetail,
                     removingItems => removingItems.Foreach(this.RemoveDetail));
    }
}
