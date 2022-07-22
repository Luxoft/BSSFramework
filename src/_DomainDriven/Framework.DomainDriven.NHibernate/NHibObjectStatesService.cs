using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL.Tracking;

using JetBrains.Annotations;

using NHibernate;
using NHibernate.Collection;
using NHibernate.Engine;

namespace Framework.DomainDriven.NHibernate
{
    internal class NHibObjectStatesService : IObjectStateService
    {
        private readonly ISession session;


        public NHibObjectStatesService([NotNull] ISession session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }


        public IEnumerable<ObjectState> GetModifiedObjectStates(object entity)
        {
            if (entity == null)
            {
                return new ObjectState[0];
            }

            if (this.session.DefaultReadOnly)
            {
                return new ObjectState[0];
            }

            var sessionImpl = this.session.GetSessionImplementation();

            var unProxy = sessionImpl.TryUnProxy(entity);

            var oldEntry = this.session.GetEntityEntry(unProxy);

            if (null == oldEntry)
            {
                return new ObjectState[0];
            }

            var persister = this.session.GetPersister(oldEntry);

            var oldState = oldEntry.LoadedState;

            var currentState = persister.GetPropertyValues(unProxy);

            var dirtyIndexies = persister.FindDirty(currentState, oldState, unProxy, sessionImpl);

            var modifiedIndexies = (dirtyIndexies ?? new int[0]).ToHashSet();

            Func<int, bool> isModifiedPropertyFunc = (index) =>
            {

                if (modifiedIndexies.Contains(index))
                {
                    return true;
                }

                var propertyType = persister.PropertyTypes[index];

                if (propertyType.IsCollectionType)
                {
                    IList<object> currentCollection = null;
                    object unTypedStateCollection = currentState[index];

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
                        currentCollection = (currentState[index] as IList<object>) ?? (((IEnumerable)currentState[index]).Cast<object>().ToList());
                    }

                    return currentCollection
                        .SelectMany(z => this.session.GetEntityEntry(z) != null ? this.GetModifiedObjectStates(z) : new[] { new ObjectState(), })
                        .Any();
                }

                return false;
            };

            return persister.PropertyNames
                .Select(TupleStruct.Create)
                .Where(z => isModifiedPropertyFunc(z.Item2))
                .Select(z =>
                {
                    var previusState = oldState[z.Item2];
                    var currentValue = currentState[z.Item2];

                    if (persister.PropertyTypes[z.Item2].IsCollectionType && currentValue is IPersistentCollection)
                    {
                        previusState = ((IPersistentCollection)currentValue).StoredSnapshot;
                    }

                    return new ObjectState(
                        z.Item1,
                        currentValue,
                        previusState,
                        true);
                });
        }

        public bool IsNew([NotNull] object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var oldEntry = this.session.GetEntityEntry(entity);

            if (null == oldEntry)
            {
                return true;
            }

            return !oldEntry.ExistsInDatabase;
        }

        public bool IsRemoving([NotNull] object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entityEntry = this.session.GetEntityEntry(entity);

            return entityEntry.Status == Status.Deleted;
        }
    }
}
