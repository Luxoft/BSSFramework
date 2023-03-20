using System.Collections.Generic;

namespace Framework.DomainDriven;

public interface ICollectionMappingService<in TSource, in TTarget> : IMappingService<IEnumerable<TSource>, IEnumerable<TTarget>>
{

}
