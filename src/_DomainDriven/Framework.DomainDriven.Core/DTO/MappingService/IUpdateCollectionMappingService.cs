using Framework.Persistent;

namespace Framework.DomainDriven
{
    public interface IUpdateCollectionMappingService<TSource, TSourceIdentity, in TTarget> : ICollectionMappingService<UpdateItemData<TSource, TSourceIdentity>, TTarget>
    {

    }
}
