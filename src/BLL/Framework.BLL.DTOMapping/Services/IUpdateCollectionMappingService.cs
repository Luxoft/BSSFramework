using Framework.BLL.DTOMapping.MergeItemData;

namespace Framework.BLL.DTOMapping.Services;

public interface IUpdateCollectionMappingService<TSource, TSourceIdentity, in TTarget> : ICollectionMappingService<UpdateItemData<TSource, TSourceIdentity>, TTarget>
{

}
