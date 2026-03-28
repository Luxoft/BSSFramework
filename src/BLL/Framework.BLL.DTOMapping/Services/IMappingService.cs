namespace Framework.BLL.DTOMapping.Services;

public interface IMappingService<in TSource, in TTarget>
{
    void Map(TSource source, TTarget target);
}
