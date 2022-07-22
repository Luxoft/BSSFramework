using System;

namespace Framework.DomainDriven.BLL;

public interface IOperationEventListener<in TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
{
    void OnFired<TDomainObject, TOperation>(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum;
}
