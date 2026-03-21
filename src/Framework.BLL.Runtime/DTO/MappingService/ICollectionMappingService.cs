namespace Framework.BLL.DTO.MappingService;

public interface ICollectionMappingService<in TSource, in TTarget> : IMappingService<IEnumerable<TSource>, IEnumerable<TTarget>>
{

}
