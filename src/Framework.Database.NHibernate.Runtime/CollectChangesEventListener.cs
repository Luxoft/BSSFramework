using NHibernate.Event;

namespace Framework.Database.NHibernate;

internal class CollectChangesEventListener : IPostDeleteEventListener, IPostInsertEventListener, IPostUpdateEventListener
{
    private readonly List<IDALObject> insertedObjects = [];
    private readonly List<IDALObject> removedObjects = [];
    private readonly List<IDALObject> updatedObjects = [];

    private int counter;

    public IEnumerable<IDALObject> InsertedObjects
    {
        get
        {
            return this.insertedObjects;
        }
    }

    public IEnumerable<IDALObject> RemovedObjects
    {
        get
        {
            return this.removedObjects;
        }
    }

    public IEnumerable<IDALObject> UpdatedObjects
    {
        get
        {
            return this.updatedObjects;
        }
    }

    public void Clear()
    {
        this.insertedObjects.Clear();
        this.removedObjects.Clear();
        this.updatedObjects.Clear();
    }

    public DALChanges EvictChanges()
    {
        var result = new DALChanges(this.insertedObjects, this.updatedObjects, this.removedObjects);
        this.Clear();
        return result;
    }

    public bool HasAny()
    {
        return this.insertedObjects.Any() || this.removedObjects.Any() || this.updatedObjects.Any();
    }

    public async Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
    {
        this.OnPostDelete(@event);
    }

    public void OnPostDelete(PostDeleteEvent @event)
    {
        if (null == @event.Persister.EntityMetamodel.Type)
        {
            return;
        }
        this.removedObjects.Add(@event.ToDALObjects(this.counter++));
    }

    public async Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
    {
        this.OnPostInsert(@event);
    }

    public void OnPostInsert(PostInsertEvent @event)
    {
        if (null == @event.Persister.EntityMetamodel.Type)
        {
            return;
        }

        this.insertedObjects.Add(@event.ToDALObjects(this.counter++));
    }

    public async Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
    {
        this.OnPostUpdate(@event);
    }

    public void OnPostUpdate(PostUpdateEvent @event)
    {
        if (null == @event.Persister.EntityMetamodel.Type)
        {
            return;
        }

        this.updatedObjects.Add(@event.ToDALObjects(this.counter++));
    }
}
