using System.Collections;

using Framework.Tracking;

using NHibernate;
using NHibernate.Collection;
using NHibernate.Engine;

namespace Framework.Database.NHibernate;

public class NHibObjectStatesService(ISession session) : IObjectStateService
{
    public IEnumerable<ObjectState> GetModifiedObjectStates(object? entity)
    {
        if (entity == null)
        {
            return Array.Empty<ObjectState>();
        }

        if (session.DefaultReadOnly)
        {
            return Array.Empty<ObjectState>();
        }

        var sessionImpl = session.GetSessionImplementation();

        var unProxy = sessionImpl.TryUnProxy(entity);

        var oldEntry = session.GetEntityEntry(unProxy);

        if (null == oldEntry)
        {
            return Array.Empty<ObjectState>();
        }

        var persister = session.GetPersister(oldEntry);

        var oldState = oldEntry.LoadedState;

        var currentState = persister.GetPropertyValues(unProxy);

        var dirtyIndexies = persister.FindDirty(currentState, oldState, unProxy, sessionImpl);

        var modifiedIndexies = (dirtyIndexies ?? []).ToHashSet();

        Func<int, bool> isModifiedPropertyFunc = (index) =>
        {

            if (modifiedIndexies.Contains(index))
            {
                return true;
            }

            var propertyType = persister.PropertyTypes[index];

            if (propertyType.IsCollectionType)
            {
                List<object>? currentCollection;
                var unTypedStateCollection = currentState[index];

                if (unTypedStateCollection is IPersistentCollection)
                {
                    var persistentCollection = (IPersistentCollection)currentState[index];
                    if (persistentCollection == null)
                    {
                        return false;
                    }
                    var result = persistentCollection.IsDirty;
                    if (result)
                    {
                        return result;
                    }

                    var collectionPersistent = sessionImpl.Factory.GetCollectionPersister(persistentCollection.Role);


                    var enumerable = persistentCollection.Entries(collectionPersistent);

                    if (null == enumerable)
                    {
                        return false;
                    }
                    currentCollection = enumerable.Cast<object>().ToList();

                }
                else
                {
                    currentCollection = (currentState[index] as List<object>) ?? (((IEnumerable)currentState[index]).Cast<object>().ToList());
                }

                return currentCollection
                       .SelectMany(z => session.GetEntityEntry(z) != null ? this.GetModifiedObjectStates(z) : new[] { new ObjectState(), })
                       .Any();
            }

            return false;
        };

        return persister.PropertyNames
                        .Select((str, index) => ValueTuple.Create(str, index))
                        .Where(z => isModifiedPropertyFunc(z.Item2))
                        .Select(z =>
                        {
                            var previousState = oldState[z.Item2];
                            var currentValue = currentState[z.Item2];

                            if (persister.PropertyTypes[z.Item2].IsCollectionType && currentValue is IPersistentCollection)
                            {
                                previousState = ((IPersistentCollection)currentValue).StoredSnapshot;
                            }

                            return new ObjectState(
                                z.Item1,
                                currentValue,
                                previousState,
                                true);
                        });
    }

    public bool IsNew(object entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var oldEntry = session.GetEntityEntry(entity);

        if (null == oldEntry)
        {
            return true;
        }

        return !oldEntry.ExistsInDatabase;
    }

    public bool IsRemoving(object entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var entityEntry = session.GetEntityEntry(entity);

        return entityEntry.Status == Status.Deleted;
    }
}
