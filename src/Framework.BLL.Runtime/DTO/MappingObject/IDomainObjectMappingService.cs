namespace Framework.BLL.DTO.MappingObject;

public interface IDomainObjectMappingService<in TPesistentDomainObjectBase>
{
    void MapToDomainObject<TDomainObject>(TDomainObject source, TDomainObject target)
            where TDomainObject : TPesistentDomainObjectBase;
}
