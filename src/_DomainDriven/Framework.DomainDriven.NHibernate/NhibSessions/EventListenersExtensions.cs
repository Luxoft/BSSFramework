using System;
using System.Linq;

using JetBrains.Annotations;

using NHibernate.Event;

namespace Framework.DomainDriven.NHibernate;

public static class EventListenersExtensions
{
    internal static EventListeners Concat([NotNull] this EventListeners eventListeners, [NotNull] EventListeners otherEventListeners)
    {
        if (eventListeners == null) throw new ArgumentNullException(nameof(eventListeners));
        if (otherEventListeners == null) throw new ArgumentNullException(nameof(otherEventListeners));

        var result = new EventListeners();

        result.PostDeleteEventListeners = eventListeners.PostDeleteEventListeners.Concat(otherEventListeners.PostDeleteEventListeners).ToArray();
        result.PostUpdateEventListeners = eventListeners.PostUpdateEventListeners.Concat(otherEventListeners.PostUpdateEventListeners).ToArray();
        result.PostInsertEventListeners = eventListeners.PostInsertEventListeners.Concat(otherEventListeners.PostInsertEventListeners).ToArray();
        result.LoadEventListeners = eventListeners.LoadEventListeners.Concat(otherEventListeners.LoadEventListeners).ToArray();
        result.SaveOrUpdateEventListeners = eventListeners.SaveOrUpdateEventListeners.Concat(otherEventListeners.SaveOrUpdateEventListeners).ToArray();
        result.MergeEventListeners = eventListeners.MergeEventListeners.Concat(otherEventListeners.MergeEventListeners).ToArray();
        result.PersistEventListeners = eventListeners.PersistEventListeners.Concat(otherEventListeners.PersistEventListeners).ToArray();
        result.PersistOnFlushEventListeners = eventListeners.PersistOnFlushEventListeners.Concat(otherEventListeners.PersistOnFlushEventListeners).ToArray();
        result.ReplicateEventListeners = eventListeners.ReplicateEventListeners.Concat(otherEventListeners.ReplicateEventListeners).ToArray();
        result.DeleteEventListeners = eventListeners.DeleteEventListeners.Concat(otherEventListeners.DeleteEventListeners).ToArray();
        result.AutoFlushEventListeners = eventListeners.AutoFlushEventListeners.Concat(otherEventListeners.AutoFlushEventListeners).ToArray();
        result.DirtyCheckEventListeners = eventListeners.DirtyCheckEventListeners.Concat(otherEventListeners.DirtyCheckEventListeners).ToArray();
        result.FlushEventListeners = eventListeners.FlushEventListeners.Concat(otherEventListeners.FlushEventListeners).ToArray();
        result.EvictEventListeners = eventListeners.EvictEventListeners.Concat(otherEventListeners.EvictEventListeners).ToArray();
        result.LockEventListeners = eventListeners.LockEventListeners.Concat(otherEventListeners.LockEventListeners).ToArray();
        result.RefreshEventListeners = eventListeners.RefreshEventListeners.Concat(otherEventListeners.RefreshEventListeners).ToArray();
        result.FlushEntityEventListeners = eventListeners.FlushEntityEventListeners.Concat(otherEventListeners.FlushEntityEventListeners).ToArray();
        result.InitializeCollectionEventListeners = eventListeners.InitializeCollectionEventListeners.Concat(otherEventListeners.InitializeCollectionEventListeners).ToArray();
        result.PostLoadEventListeners = eventListeners.PostLoadEventListeners.Concat(otherEventListeners.PostLoadEventListeners).ToArray();
        result.PreLoadEventListeners = eventListeners.PreLoadEventListeners.Concat(otherEventListeners.PreLoadEventListeners).ToArray();
        result.PreDeleteEventListeners = eventListeners.PreDeleteEventListeners.Concat(otherEventListeners.PreDeleteEventListeners).ToArray();
        result.PreUpdateEventListeners = eventListeners.PreUpdateEventListeners.Concat(otherEventListeners.PreUpdateEventListeners).ToArray();
        result.PreInsertEventListeners = eventListeners.PreInsertEventListeners.Concat(otherEventListeners.PreInsertEventListeners).ToArray();
        result.PostCommitDeleteEventListeners = eventListeners.PostCommitDeleteEventListeners.Concat(otherEventListeners.PostCommitDeleteEventListeners).ToArray();
        result.PostCommitUpdateEventListeners = eventListeners.PostCommitUpdateEventListeners.Concat(otherEventListeners.PostCommitUpdateEventListeners).ToArray();
        result.PostCommitInsertEventListeners = eventListeners.PostCommitInsertEventListeners.Concat(otherEventListeners.PostCommitInsertEventListeners).ToArray();
        result.PreCollectionRecreateEventListeners = eventListeners.PreCollectionRecreateEventListeners.Concat(otherEventListeners.PreCollectionRecreateEventListeners).ToArray();
        result.PostCollectionRecreateEventListeners = eventListeners.PostCollectionRecreateEventListeners.Concat(otherEventListeners.PostCollectionRecreateEventListeners).ToArray();
        result.PreCollectionRemoveEventListeners = eventListeners.PreCollectionRemoveEventListeners.Concat(otherEventListeners.PreCollectionRemoveEventListeners).ToArray();
        result.PostCollectionRemoveEventListeners = eventListeners.PostCollectionRemoveEventListeners.Concat(otherEventListeners.PostCollectionRemoveEventListeners).ToArray();
        result.PreCollectionUpdateEventListeners = eventListeners.PreCollectionUpdateEventListeners.Concat(otherEventListeners.PreCollectionUpdateEventListeners).ToArray();
        result.PostCollectionUpdateEventListeners = eventListeners.PostCollectionUpdateEventListeners.Concat(otherEventListeners.PostCollectionUpdateEventListeners).ToArray();
        result.SaveEventListeners = eventListeners.SaveEventListeners.Concat(otherEventListeners.SaveEventListeners).ToArray();
        result.UpdateEventListeners = eventListeners.UpdateEventListeners.Concat(otherEventListeners.UpdateEventListeners).ToArray();

        return result;
    }
}
