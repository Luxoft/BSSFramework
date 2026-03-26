namespace Framework.BLL.DTOMapping;

public interface ICollectionMappingService<in TSource, in TTarget> : IMappingService<IEnumerable<TSource>, IEnumerable<TTarget>>
{

}
