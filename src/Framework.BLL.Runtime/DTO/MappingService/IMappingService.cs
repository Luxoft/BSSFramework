

namespace Framework.BLL.DTO.MappingService;

public interface IMappingService<in TSource, in TTarget>
{
    void Map(TSource source, TTarget target);
}
