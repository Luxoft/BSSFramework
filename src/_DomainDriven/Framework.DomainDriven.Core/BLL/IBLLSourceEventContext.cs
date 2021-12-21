namespace Framework.DomainDriven.BLL
{
    public interface IBLLSourceEventContext<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
    {
        BLLSourceEventListenerContainer<TPersistentDomainObjectBase> SourceListeners { get; }
    }
}