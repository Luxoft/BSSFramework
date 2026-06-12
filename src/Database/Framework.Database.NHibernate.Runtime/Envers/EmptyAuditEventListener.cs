using NHibernate.Event;

namespace Framework.Database.NHibernate.Envers;

public class EmptyAuditEventListener : IPostInsertEventListener,
                                       IPostUpdateEventListener,
                                       IPostDeleteEventListener,
                                       IPreCollectionUpdateEventListener,
                                       IPreCollectionRemoveEventListener,
                                       IPostCollectionRecreateEventListener,
                                       IInitializable
{
    public async Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken ct)
    {
    }

    public void OnPostInsert(PostInsertEvent @event)
    {
        // Empty
    }

    public async Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken ct)
    {
    }

    public void OnPostUpdate(PostUpdateEvent @event)
    {
        // Empty
    }

    public async Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken ct)
    {
    }

    public void OnPostDelete(PostDeleteEvent @event)
    {
        // Empty
    }

    public async Task OnPreUpdateCollectionAsync(PreCollectionUpdateEvent @event, CancellationToken ct)
    {
    }

    public void OnPreUpdateCollection(PreCollectionUpdateEvent @event)
    {
        // Empty
    }

    public async Task OnPreRemoveCollectionAsync(PreCollectionRemoveEvent @event, CancellationToken ct)
    {
    }

    public void OnPreRemoveCollection(PreCollectionRemoveEvent @event)
    {
        // Empty
    }

    public async Task OnPostRecreateCollectionAsync(PostCollectionRecreateEvent @event, CancellationToken ct)
    {
    }

    public void OnPostRecreateCollection(PostCollectionRecreateEvent @event)
    {
        // Empty
    }

    public void Initialize(global::NHibernate.Cfg.Configuration cfg)
    {
        // Empty
    }
}

