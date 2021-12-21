namespace Framework.DomainDriven
{
    public interface IDomainObjectMappingService<in TPesistentDomainObjectBase>
    {
        void MapToDomainObject<TDomainObject>(TDomainObject source, TDomainObject target)
            where TDomainObject : TPesistentDomainObjectBase;
    }
}