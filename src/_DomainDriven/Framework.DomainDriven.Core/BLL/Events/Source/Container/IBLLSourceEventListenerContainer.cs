namespace Framework.DomainDriven.BLL
{
    public interface IBLLSourceEventListenerContainer<in TPersistentDomainObjectBase>
    {
        IBLLSourceEventListener<TDomainObject> GetEventListener<TDomainObject>()
            where TDomainObject : TPersistentDomainObjectBase;
    }
}