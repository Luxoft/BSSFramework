namespace Framework.BLL.DTOMapping;

public interface IMappingService<in TSource, in TTarget>
{
    void Map(TSource source, TTarget target);
}
