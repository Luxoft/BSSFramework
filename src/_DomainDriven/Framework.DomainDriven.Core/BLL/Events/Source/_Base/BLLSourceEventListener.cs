using System;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public sealed class BLLSourceEventListener<TDomainObject> : IBLLSourceEventListener<TDomainObject>
    {
        internal BLLSourceEventListener()
        {

        }


        internal void InvokeObjectSaving(EventArgsWithCancel<TDomainObject> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.ObjectSaving.Maybe(handler => handler(this, eventArgs));
        }

        internal void InvokeObjectSaved(EventArgs<TDomainObject> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.ObjectSaved.Maybe(handler => handler(this, eventArgs));
        }

        internal void InvokeObjectRemoving(EventArgsWithCancel<TDomainObject> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.ObjectRemoving.Maybe(handler => handler(this, eventArgs));
        }

        internal void InvokeObjectRemoved(EventArgs<TDomainObject> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.ObjectRemoved.Maybe(handler => handler(this, eventArgs));
        }

        internal void InvokeObjectsQueried(ObjectsQueriedEventArgs<TDomainObject> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.ObjectsQueried.Maybe(handler => handler(this, eventArgs));
        }


        public event EventHandler<EventArgsWithCancel<TDomainObject>> ObjectSaving;

        public event EventHandler<EventArgs<TDomainObject>> ObjectSaved;

        public event EventHandler<EventArgsWithCancel<TDomainObject>> ObjectRemoving;

        public event EventHandler<EventArgs<TDomainObject>> ObjectRemoved;

        public event EventHandler<ObjectsQueriedEventArgs<TDomainObject>> ObjectsQueried;
    }
}