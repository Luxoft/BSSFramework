using System.Collections.ObjectModel;

using CommonFramework;
using CommonFramework.DictionaryCache;

using Framework.Application.Session.DALObject;
using Framework.Core;

namespace Framework.Application.Session.DALChanges;

public class DalChanges<T>
{
    public DalChanges(IEnumerable<T> createdItems, IEnumerable<T> updatedItems, IEnumerable<T> removedItems)
    {
        if (createdItems == null) throw new ArgumentNullException(nameof(createdItems));
        if (updatedItems == null) throw new ArgumentNullException(nameof(updatedItems));
        if (removedItems == null) throw new ArgumentNullException(nameof(removedItems));

        this.CreatedItems = createdItems.ToReadOnlyCollection();
        this.UpdatedItems = updatedItems.ToReadOnlyCollection();
        this.RemovedItems = removedItems.ToReadOnlyCollection();
    }

    public DalChanges(IReadOnlyDictionary<T, DalObjectChangeType> dalChanges)
    {
        if (dalChanges == null) throw new ArgumentNullException(nameof(dalChanges));

        this.CreatedItems = dalChanges.Where(pair => pair.Value == DalObjectChangeType.Created).ToReadOnlyCollection(pair => pair.Key);
        this.UpdatedItems = dalChanges.Where(pair => pair.Value == DalObjectChangeType.Updated).ToReadOnlyCollection(pair => pair.Key);
        this.RemovedItems = dalChanges.Where(pair => pair.Value == DalObjectChangeType.Removed).ToReadOnlyCollection(pair => pair.Key);
    }


    public bool IsEmpty => !this.CreatedItems.Any() && !this.UpdatedItems.Any() && !this.RemovedItems.Any();


    public ReadOnlyCollection<T> CreatedItems { get; }

    public ReadOnlyCollection<T> UpdatedItems { get; }

    public ReadOnlyCollection<T> RemovedItems { get; }
}

public class DalChanges : DalChanges<IdalObject>
{
    private readonly IDictionaryCache<Type, DalChanges> subsetCache;

    private readonly Lazy<Dictionary<Type, DalChanges<IdalObject>>> lazyGroupDalObjectByType;

    private readonly Lazy<Dictionary<Type, DalChanges<object>>> lazyGroupByType;


    public DalChanges(DalChanges<IdalObject> dalChanges)
            : this(dalChanges.CreatedItems, dalChanges.UpdatedItems, dalChanges.RemovedItems)
    {
    }

    public DalChanges(IReadOnlyDictionary<IdalObject, DalObjectChangeType> dalChanges)
            : this(
                   dalChanges.Where(pair => pair.Value == DalObjectChangeType.Created).ToReadOnlyCollection(pair => pair.Key),
                   dalChanges.Where(pair => pair.Value == DalObjectChangeType.Updated).ToReadOnlyCollection(pair => pair.Key),
                   dalChanges.Where(pair => pair.Value == DalObjectChangeType.Removed).ToReadOnlyCollection(pair => pair.Key))
    {
    }

    public DalChanges(IEnumerable<IdalObject> createdItems, IEnumerable<IdalObject> updatedItems, IEnumerable<IdalObject> removedItems)
            : base(createdItems, updatedItems, removedItems)
    {
        this.subsetCache = new DictionaryCache<Type, DalChanges>(t => new DalChanges(this.Where(dalObject => t.IsAssignableFrom(dalObject.Type)))).WithLock();

        this.lazyGroupDalObjectByType = LazyHelper.Create(() =>
                                                           {
                                                               var plainValues = this.ToPlainValues();

                                                               var grouped = plainValues.GroupBy(z => z.Item1.Type);

                                                               return grouped.ToDictionary(z => z.Key, q => q.Partial(z => z.Item2 == DalObjectChangeType.Created, z => z.Item2 == DalObjectChangeType.Updated, (cr, upd, rem) => new DalChanges<IdalObject>(cr.Select(e => e.Item1), upd.Select(e => e.Item1), rem.Select(e => e.Item1))));
                                                           });

        this.lazyGroupByType = LazyHelper.Create(() =>
                                                  {
                                                      var request = from pair in this.ToChangeTypeDict()

                                                                    group new { pair.Key.Object, pair.Value } by pair.Key.Type into typeGroup

                                                                    select new
                                                                           {
                                                                                   typeGroup.Key,
                                                                                   Value = new DalChanges<object>(
                                                                                    typeGroup.Where(pair => pair.Value == DalObjectChangeType.Created).Select(pair => pair.Object),
                                                                                    typeGroup.Where(pair => pair.Value == DalObjectChangeType.Updated).Select(pair => pair.Object),
                                                                                    typeGroup.Where(pair => pair.Value == DalObjectChangeType.Removed).Select(pair => pair.Object))
                                                                           };

                                                      return request.ToDictionary(pair => pair.Key, pair => pair.Value);
                                                  });
    }

    public DalChanges GetSubset(Type type)
    {
        return this.subsetCache[type];
    }


    public Dictionary<Type, DalChanges<IdalObject>> GroupDalObjectByType()
    {
        return this.lazyGroupDalObjectByType.Value;
    }

    public Dictionary<Type, DalChanges<object>> GroupByType()
    {
        return this.lazyGroupByType.Value;
    }
}
