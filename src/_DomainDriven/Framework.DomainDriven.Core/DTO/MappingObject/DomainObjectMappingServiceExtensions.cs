namespace Framework.DomainDriven
{
    public static class DomainObjectMappingServiceExtensions
    {
        public static TDomainObject Clone<TPesistentDomainObjectBase, TDomainObject> (this IDomainObjectMappingService<TPesistentDomainObjectBase> mappingService, TDomainObject source)
            where TDomainObject : TPesistentDomainObjectBase, new()
        {
            var target = new TDomainObject();

            mappingService.MapToDomainObject(source, target);

            return target;
        }
    }
}