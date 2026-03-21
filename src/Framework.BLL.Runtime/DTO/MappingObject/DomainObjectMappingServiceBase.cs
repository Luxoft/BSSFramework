using Framework.BLL.BLL;

namespace Framework.BLL.DTO.MappingObject;

public abstract class DomainObjectMappingServiceBase<TBLLContext, TPesistentDomainObjectBase> :
        BLLContextContainer<TBLLContext>, IDomainObjectMappingService<TPesistentDomainObjectBase>
        where TBLLContext : class
{
    protected DomainObjectMappingServiceBase(TBLLContext context)
            : base(context)
    {

    }


    public abstract void MapToDomainObject<TDomainObject>(TDomainObject source, TDomainObject target)
            where TDomainObject : TPesistentDomainObjectBase;
}
