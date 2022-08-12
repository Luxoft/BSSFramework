using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven
{
    public class DALChanges<T>
    {
        public DALChanges([NotNull] IEnumerable<T> createdItems, [NotNull] IEnumerable<T> updatedItems, [NotNull] IEnumerable<T> removedItems)
        {
            if (createdItems == null) throw new ArgumentNullException(nameof(createdItems));
            if (updatedItems == null) throw new ArgumentNullException(nameof(updatedItems));
            if (removedItems == null) throw new ArgumentNullException(nameof(removedItems));

            this.CreatedItems = createdItems.ToReadOnlyCollection();
            this.UpdatedItems = updatedItems.ToReadOnlyCollection();
            this.RemovedItems = removedItems.ToReadOnlyCollection();
        }

        public DALChanges([NotNull] IReadOnlyDictionary<T, DALObjectChangeType> dalChanges)
        {
            if (dalChanges == null) throw new ArgumentNullException(nameof(dalChanges));

            this.CreatedItems = dalChanges.Where(pair => pair.Value == DALObjectChangeType.Created).ToReadOnlyCollection(pair => pair.Key);
            this.UpdatedItems = dalChanges.Where(pair => pair.Value == DALObjectChangeType.Updated).ToReadOnlyCollection(pair => pair.Key);
            this.RemovedItems = dalChanges.Where(pair => pair.Value == DALObjectChangeType.Removed).ToReadOnlyCollection(pair => pair.Key);
        }


        public bool IsEmpty => !this.CreatedItems.Any() && !this.UpdatedItems.Any() && !this.RemovedItems.Any();


        public ReadOnlyCollection<T> CreatedItems { get; }

        public ReadOnlyCollection<T> UpdatedItems { get; }

        public ReadOnlyCollection<T> RemovedItems { get; }
    }

    public class DALChanges : DALChanges<IDALObject>
    {
        private readonly IDictionaryCache<Type, DALChanges> _subsetCache;

        private readonly Lazy<Dictionary<Type, DALChanges<IDALObject>>> _lazyGroupDALObjectByType;

        private readonly Lazy<Dictionary<Type, DALChanges<object>>> _lazyGroupByType;


        public DALChanges(DALChanges<IDALObject> dalChanges)
            : this(dalChanges.CreatedItems, dalChanges.UpdatedItems, dalChanges.RemovedItems)
        {
        }

        public DALChanges(IReadOnlyDictionary<IDALObject, DALObjectChangeType> dalChanges)
            : this(
                dalChanges.Where(pair => pair.Value == DALObjectChangeType.Created).ToReadOnlyCollection(pair => pair.Key),
                dalChanges.Where(pair => pair.Value == DALObjectChangeType.Updated).ToReadOnlyCollection(pair => pair.Key),
                dalChanges.Where(pair => pair.Value == DALObjectChangeType.Removed).ToReadOnlyCollection(pair => pair.Key))
        {
        }

        public DALChanges([NotNull] IEnumerable<IDALObject> createdItems, [NotNull] IEnumerable<IDALObject> updatedItems, [NotNull] IEnumerable<IDALObject> removedItems)
            : base(createdItems, updatedItems, removedItems)
        {
            this._subsetCache = new DictionaryCache<Type, DALChanges>(t => new DALChanges(this.Where(dalObject => t.IsAssignableFrom(dalObject.Type)))).WithLock();

            this._lazyGroupDALObjectByType = LazyHelper.Create(() =>
            {
                var plainValues = this.ToPlainValues();

                var grouped = plainValues.GroupBy(z => z.Item1.Type);

                return grouped.ToDictionary(z => z.Key, q => q.Partial(z => z.Item2 == DALObjectChangeType.Created, z => z.Item2 == DALObjectChangeType.Updated, (cr, upd, rem) => new DALChanges<IDALObject>(cr.Select(e => e.Item1), upd.Select(e => e.Item1), rem.Select(e => e.Item1))));
            });

            this._lazyGroupByType = LazyHelper.Create(() =>
            {
                var request = from pair in this.ToChangeTypeDict()

                              group new { pair.Key.Object, pair.Value } by pair.Key.Type into typeGroup

                              select new
                              {
                                  typeGroup.Key,
                                  Value = new DALChanges<object>(
                                      typeGroup.Where(pair => pair.Value == DALObjectChangeType.Created).Select(pair => pair.Object),
                                      typeGroup.Where(pair => pair.Value == DALObjectChangeType.Updated).Select(pair => pair.Object),
                                      typeGroup.Where(pair => pair.Value == DALObjectChangeType.Removed).Select(pair => pair.Object))
                              };

                return request.ToDictionary(pair => pair.Key, pair => pair.Value);
            });
        }

        public DALChanges GetSubset(Type type)
        {
            return this._subsetCache[type];
        }


        public Dictionary<Type, DALChanges<IDALObject>> GroupDALObjectByType()
        {
            return this._lazyGroupDALObjectByType.Value;
        }

        public Dictionary<Type, DALChanges<object>> GroupByType()
        {
            return this._lazyGroupByType.Value;
        }
    }
}
