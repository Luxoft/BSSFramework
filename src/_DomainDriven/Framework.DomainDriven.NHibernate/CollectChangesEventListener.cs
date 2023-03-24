using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.DomainDriven.BLL;
using NHibernate.Event;

namespace Framework.DomainDriven.NHibernate;

internal class CollectChangesEventListener : IPostDeleteEventListener, IPostInsertEventListener, IPostUpdateEventListener
{
    private readonly IList<IDALObject> _insertedObjects;
    private readonly IList<IDALObject> _removedObjects;
    private readonly IList<IDALObject> _updatedObjects;

    private int counter;

    internal CollectChangesEventListener()
    {
        this._insertedObjects = new List<IDALObject>();
        this._removedObjects = new List<IDALObject>();
        this._updatedObjects = new List<IDALObject>();
    }

    public IEnumerable<IDALObject> InsertedObjects
    {
        get
        {
            return this._insertedObjects;
        }
    }

    public IEnumerable<IDALObject> RemovedObjects
    {
        get
        {
            return this._removedObjects;
        }
    }

    public IEnumerable<IDALObject> UpdatedObjects
    {
        get
        {
            return this._updatedObjects;
        }
    }

    public void Clear()
    {
        this._insertedObjects.Clear();
        this._removedObjects.Clear();
        this._updatedObjects.Clear();
    }

    public DALChanges EvictChanges()
    {
        var result = new DALChanges(this._insertedObjects, this._updatedObjects, this._removedObjects);
        this.Clear();
        return result;
    }

    public bool HasAny()
    {
        return this._insertedObjects.Any() || this._removedObjects.Any() || this._updatedObjects.Any();
    }

    public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
    {
        this.OnPostDelete(@event);

        return Task.CompletedTask;
    }

    public void OnPostDelete(PostDeleteEvent @event)
    {
        if (null == @event.Persister.EntityMetamodel.Type)
        {
            return;
        }
        this._removedObjects.Add(@event.ToDALObjects(this.counter++));
    }

    public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
    {
        this.OnPostInsert(@event);
        return Task.CompletedTask;
    }

    public void OnPostInsert(PostInsertEvent @event)
    {
        if (null == @event.Persister.EntityMetamodel.Type)
        {
            return;
        }

        this._insertedObjects.Add(@event.ToDALObjects(this.counter++));
    }

    public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
    {
        this.OnPostUpdate(@event);
        return Task.CompletedTask;
    }

    public void OnPostUpdate(PostUpdateEvent @event)
    {
        if (null == @event.Persister.EntityMetamodel.Type)
        {
            return;
        }

        this._updatedObjects.Add(@event.ToDALObjects(this.counter++));
    }
}
