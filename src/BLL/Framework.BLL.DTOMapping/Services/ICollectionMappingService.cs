namespace Framework.BLL.DTOMapping.Services;

public interface ICollectionMappingService<in TSource, in TTarget> : IMappingService<IEnumerable<TSource>, IEnumerable<TTarget>>;
