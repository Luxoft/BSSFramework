using NHibernate.Event;

namespace NHibernate.Envers.Patch;

public class EmptyAuditEventListener : IPostInsertEventListener,
                                       IPostUpdateEventListener,
                                       IPostDeleteEventListener,
                                       IPreCollectionUpdateEventListener,
                                       IPreCollectionRemoveEventListener,
                                       IPostCollectionRecreateEventListener,
                                       IInitializable
{
    public async Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
    {
    }

    public void OnPostInsert(PostInsertEvent @event)
    {
        // Empty
    }

    public async Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
    {
    }

    public void OnPostUpdate(PostUpdateEvent @event)
    {
        // Empty
    }

    public async Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
    {
    }

    public void OnPostDelete(PostDeleteEvent @event)
    {
        // Empty
    }

    public async Task OnPreUpdateCollectionAsync(PreCollectionUpdateEvent @event, CancellationToken cancellationToken)
    {
    }

    public void OnPreUpdateCollection(PreCollectionUpdateEvent @event)
    {
        // Empty
    }

    public async Task OnPreRemoveCollectionAsync(PreCollectionRemoveEvent @event, CancellationToken cancellationToken)
    {
    }

    public void OnPreRemoveCollection(PreCollectionRemoveEvent @event)
    {
        // Empty
    }

    public async Task OnPostRecreateCollectionAsync(PostCollectionRecreateEvent @event, CancellationToken cancellationToken)
    {
    }

    public void OnPostRecreateCollection(PostCollectionRecreateEvent @event)
    {
        // Empty
    }

    public void Initialize(Cfg.Configuration cfg)
    {
        // Empty
    }
}
