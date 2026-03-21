using Framework.BLL.Domain.MergeItemData;

namespace Framework.BLL.DTO.MappingService;

public interface IUpdateCollectionMappingService<TSource, TSourceIdentity, in TTarget> : ICollectionMappingService<UpdateItemData<TSource, TSourceIdentity>, TTarget>
{

}
