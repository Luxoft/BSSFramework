using System;

namespace Framework.DomainDriven.BLL
{
    public interface IBLLSourceEventListener<TDomainObject>
    {
        event EventHandler<EventArgsWithCancel<TDomainObject>> ObjectSaving;

        event EventHandler<EventArgs<TDomainObject>> ObjectSaved;

        event EventHandler<EventArgsWithCancel<TDomainObject>> ObjectRemoving;

        event EventHandler<EventArgs<TDomainObject>> ObjectRemoved;

        event EventHandler<ObjectsQueriedEventArgs<TDomainObject>> ObjectsQueried;
    }
}