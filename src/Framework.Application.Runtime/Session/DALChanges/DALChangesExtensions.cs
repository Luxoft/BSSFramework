using Framework.Application.Session.DALObject;
using Framework.Core;

namespace Framework.Application.Session.DALChanges;

public static class DalChangesExtensions
{
    public static DalChanges<TResult> Select<TSource, TResult>(this DalChanges<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new DalChanges<TResult>(source.CreatedItems.Select(selector), source.UpdatedItems.Select(selector), source.RemovedItems.Select(selector));
    }

    public static DalChanges<T> Where<T>(this DalChanges<T> source, Func<T, bool> filter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new DalChanges<T>(source.CreatedItems.Where(filter), source.UpdatedItems.Where(filter), source.RemovedItems.Where(filter));
    }

    public static Dictionary<T, DalObjectChangeType> ToChangeTypeDict<T>(this DalChanges<T> source)
        where T : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var request = source.CreatedItems.Select(item => new { Key = item, Value = DalObjectChangeType.Created })
                            .Concat(source.UpdatedItems.Select(item => new { Key = item, Value = DalObjectChangeType.Updated }))
                            .Concat(source.RemovedItems.Select(item => new { Key = item, Value = DalObjectChangeType.Removed }));

        return request.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public static IEnumerable<ValueTuple<T, DalObjectChangeType>> ToPlainValues<T>(this DalChanges<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var combined = source.CreatedItems.Select(item => ValueTuple.Create(item, DalObjectChangeType.Created))
                             .Concat(source.UpdatedItems.Select(item => ValueTuple.Create(item, DalObjectChangeType.Updated)))
                             .Concat(source.RemovedItems.Select(item => ValueTuple.Create(item, DalObjectChangeType.Removed)));

        return combined;
    }

    /// <summary>
    /// Схлопывание коллекции изменений по объекту
    /// </summary>
    public static DalChanges Composite(this IEnumerable<DalChanges> source)
    {
        var request = from dalChanges in source

                      from pair in dalChanges.ToChangeTypeDict()

                      group (pair.Key, pair.Value) by (pair.Key.Object, pair.Key.Type) into changeGroup

                      let finalState = changeGroup.ToDictionary().ToFinalState()

                      where finalState != null

                      select finalState.Value;

        return new DalChanges(request.ToDictionary());
    }

    /// <summary>
    /// Приведение череды изменения объекта к конечному состоянию
    /// </summary>
    /// <param name="states">Череда изменений объекта</param>
    private static KeyValuePair<IdalObject, DalObjectChangeType>? ToFinalState(this IReadOnlyDictionary<IdalObject, DalObjectChangeType> states)
    {
        if (states == null) throw new ArgumentNullException(nameof(states));

        var sourceCache = states.ToArray();

        var createState = states.SingleOrDefault(state => state.Value == DalObjectChangeType.Created);

        var removeState = states.SingleOrDefault(state => state.Value == DalObjectChangeType.Removed);

        if (!sourceCache.Any())
        {
            return null;
        }
        else if (!removeState.IsDefault())
        {
            if (!createState.IsDefault())
            {
                return null;
            }
            else
            {
                return removeState;
            }
        }
        else if (!createState.IsDefault())
        {
            return createState;
        }
        else
        {
            var updateStates = states.Where(state => state.Value == DalObjectChangeType.Updated).OrderByDescending(state => state.Key.ApplyIndex);

            return updateStates.First();
        }
    }
}
