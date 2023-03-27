using JetBrains.Annotations;

using NHibernate.Event;

namespace Framework.DomainDriven.NHibernate;

public static class EventListenersExtensions
{
    internal static EventListeners Clone([NotNull] this EventListeners eventListeners)
    {
        if (eventListeners == null) throw new ArgumentNullException(nameof(eventListeners));

        return new EventListeners
        {
            PostDeleteEventListeners = eventListeners.PostDeleteEventListeners,
            PostUpdateEventListeners = eventListeners.PostUpdateEventListeners,
            PostInsertEventListeners = eventListeners.PostInsertEventListeners,
            PostCollectionRecreateEventListeners = eventListeners.PostCollectionRecreateEventListeners,
            PreCollectionRemoveEventListeners = eventListeners.PreCollectionRemoveEventListeners,
            PreCollectionUpdateEventListeners = eventListeners.PreCollectionUpdateEventListeners,
            LoadEventListeners = eventListeners.LoadEventListeners,
            SaveOrUpdateEventListeners = eventListeners.SaveOrUpdateEventListeners,
            MergeEventListeners = eventListeners.MergeEventListeners,
            PersistEventListeners = eventListeners.PersistEventListeners,
            PersistOnFlushEventListeners = eventListeners.PersistOnFlushEventListeners,
            ReplicateEventListeners = eventListeners.ReplicateEventListeners,
            DeleteEventListeners = eventListeners.DeleteEventListeners,
            AutoFlushEventListeners = eventListeners.AutoFlushEventListeners,
            DirtyCheckEventListeners = eventListeners.DirtyCheckEventListeners,
            FlushEventListeners = eventListeners.FlushEventListeners,
            EvictEventListeners = eventListeners.EvictEventListeners,
            LockEventListeners = eventListeners.LockEventListeners,
            RefreshEventListeners = eventListeners.RefreshEventListeners,
            PostCommitUpdateEventListeners = eventListeners.PostCommitUpdateEventListeners,
            PostCommitInsertEventListeners = eventListeners.PostCommitInsertEventListeners,
            PreCollectionRecreateEventListeners = eventListeners.PreCollectionRecreateEventListeners,
            PostCollectionRemoveEventListeners = eventListeners.PostCollectionRemoveEventListeners,
            PostCollectionUpdateEventListeners = eventListeners.PostCollectionUpdateEventListeners,
            SaveEventListeners = eventListeners.SaveEventListeners,
            UpdateEventListeners = eventListeners.UpdateEventListeners,
            PreUpdateEventListeners = eventListeners.PreUpdateEventListeners,
            PreInsertEventListeners = eventListeners.PreInsertEventListeners,
            PostCommitDeleteEventListeners = eventListeners.PostCommitDeleteEventListeners,
            InitializeCollectionEventListeners = eventListeners.InitializeCollectionEventListeners,
            PostLoadEventListeners = eventListeners.PostLoadEventListeners,
            PreLoadEventListeners = eventListeners.PreLoadEventListeners,
            PreDeleteEventListeners = eventListeners.PreDeleteEventListeners,
            FlushEntityEventListeners = eventListeners.FlushEntityEventListeners
        };
    }
}
