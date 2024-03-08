namespace Framework.DomainDriven.BLL;

public interface IOperationEventSenderContainer<in TPersistentDomainObjectBase>
{
    OperationEventSender<TDomainObject> GetEventSender<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase;
}
