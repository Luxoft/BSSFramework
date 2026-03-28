using CommonFramework;

using Framework.Application.Domain;
using Framework.Core;

namespace Framework.BLL.DTOMapping.Services;

public class DefaultCollectionMappingService<TSource, TTarget, TIdent>(Func<TSource, TTarget> createAndMapDetail, Action<TTarget> removeDetail)
    : ICollectionMappingService<TSource, TTarget>
    where TSource : IIdentityObject<TIdent>
    where TTarget : class, IIdentityObject<TIdent>
{
    protected virtual TTarget AddDetail(TSource source) => createAndMapDetail(source);

    protected virtual void RemoveDetail(TTarget target) => removeDetail(target);

    protected virtual TIdent GetSourceId(TSource source) => source.Id;

    protected virtual TIdent GetTargetId(TTarget target) => target.Id;

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
