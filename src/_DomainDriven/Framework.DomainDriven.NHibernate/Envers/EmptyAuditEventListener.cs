using System.Threading;
using System.Threading.Tasks;
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
    public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void OnPostInsert(PostInsertEvent @event)
    {
        // Empty
    }

    public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void OnPostUpdate(PostUpdateEvent @event)
    {
        // Empty
    }

    public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void OnPostDelete(PostDeleteEvent @event)
    {
        // Empty
    }

    public Task OnPreUpdateCollectionAsync(PreCollectionUpdateEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void OnPreUpdateCollection(PreCollectionUpdateEvent @event)
    {
        // Empty
    }

    public Task OnPreRemoveCollectionAsync(PreCollectionRemoveEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void OnPreRemoveCollection(PreCollectionRemoveEvent @event)
    {
        // Empty
    }

    public Task OnPostRecreateCollectionAsync(PostCollectionRecreateEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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
