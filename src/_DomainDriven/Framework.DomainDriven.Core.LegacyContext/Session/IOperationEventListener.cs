namespace Framework.DomainDriven;

public interface IOperationEventListener<in TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class
{
    void OnFired<TDomainObject>(IDomainOperationEventArgs<TDomainObject> eventArgs)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
