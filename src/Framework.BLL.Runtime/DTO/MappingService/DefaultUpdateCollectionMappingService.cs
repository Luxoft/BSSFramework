using Framework.Application.Domain;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.IdentityObject;
using Framework.BLL.Domain.MergeItemData;

namespace Framework.BLL.DTO.MappingService;

public class DefaultUpdateCollectionMappingService<TSource, TSourceIdentity, TTarget, TIdent> : IUpdateCollectionMappingService<TSource, TSourceIdentity, TTarget>

        where TSource : class, IIdentityObjectContainer<TSourceIdentity>

        where TTarget : class, IIdentityObject<TIdent>

        where TSourceIdentity : IIdentityObject<TIdent>
{
    private readonly Func<TSource, TTarget> createAndMapDetail;
    private readonly Action<TTarget> removeDetail;


    public DefaultUpdateCollectionMappingService(Func<TSource, TTarget> createAndMapDetail, Action<TTarget> removeDetail)
    {
        this.createAndMapDetail = createAndMapDetail ?? throw new ArgumentNullException(nameof(createAndMapDetail));
        this.removeDetail = removeDetail ?? throw new ArgumentNullException(nameof(removeDetail));
    }


    protected virtual TTarget AddDetail(TSource source)
    {
        return this.createAndMapDetail(source);
    }

    protected virtual void RemoveDetail(TTarget target)
    {
        this.removeDetail(target);
    }

    protected virtual TIdent GetSourceId(TSource source)
    {
        return this.GetSourceIdentityId(source.Identity);
    }

    protected virtual TIdent GetSourceIdentityId(TSourceIdentity sourceIdentity)
    {
        return sourceIdentity.Id;
    }

    protected virtual TIdent GetTargetId(TTarget target)
    {
        return target.Id;
    }


    public void Map(IEnumerable<UpdateItemData<TSource, TSourceIdentity>> sourceItems, IEnumerable<TTarget> targetItems)
    {
        if (sourceItems == null) throw new ArgumentNullException(nameof(sourceItems));
        if (targetItems == null) throw new ArgumentNullException(nameof(targetItems));

        sourceItems.Update(targetItems, this.GetSourceId, this.GetSourceIdentityId, this.GetTargetId, this.AddDetail, this.RemoveDetail);
    }
}
