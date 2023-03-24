using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Persistent;

public static class UpdateExtensions
{
    public static void Update<TSource, TSourceIdentity, TTarget, TKey>(
            this IEnumerable<UpdateItemData<TSource, TSourceIdentity>> source,
            IEnumerable<TTarget> target,
            Func<TSource, TKey> getSourceKey,
            Func<TSourceIdentity, TKey> getSourceIdentityKey,
            Func<TTarget, TKey> getTargetKey,
            Func<TSource, TTarget> createAndMapFunc,
            Action<TTarget> removeAction,
            IEqualityComparer<TKey> keyComparer = null)
    {
        var actualComparer = keyComparer ?? new EqualityComparerImpl<TKey>((key1, key2) => !key1.IsDefault() && !key2.IsDefault() && EqualityComparer<TKey>.Default.Equals(key1, key2));

        var targetMap = target.ToDictionary(getTargetKey, actualComparer);

        var updateLists = source.Partial(itemData => itemData is SaveItemData<TSource, TSourceIdentity>, (saveItems, removeItems) => new
                                             {
                                                     SaveItems = saveItems.Cast<SaveItemData<TSource, TSourceIdentity>>().ToList(item => item.Value),
                                                     RemoveItems = removeItems.Cast<RemoveItemData<TSource, TSourceIdentity>>().ToList(item => item.Identity)
                                             });


        foreach (var savingSourceItem in updateLists.SaveItems)
        {
            createAndMapFunc(savingSourceItem);
        }

        foreach (var removingSourceItem in updateLists.RemoveItems)
        {
            TTarget removingTargetItem;

            if (targetMap.TryGetValue(getSourceIdentityKey(removingSourceItem), out removingTargetItem))
            {
                removeAction(removingTargetItem);
            }
        }
    }

    public static IEnumerable<UpdateItemData<TTarget, TIdentity>> ExtractUpdateData<TSource, TIdentity, TTarget>(
            this IEnumerable<TSource> currentSource,
            IEnumerable<TSource> baseSource,
            Func<TSource, TSource, TTarget> getTarget,
            Func<TSource, TIdentity> getIdentity,
            IEqualityComparer<TIdentity> identityComparer = null)
            where TSource : class
    {
        var mergeResult = baseSource.GetMergeResult(currentSource, getIdentity, getIdentity);

        foreach (var removeItem in mergeResult.RemovingItems)
        {
            yield return new RemoveItemData<TTarget, TIdentity>(getIdentity(removeItem));
        }

        foreach (var addItem in mergeResult.AddingItems)
        {
            yield return new SaveItemData<TTarget, TIdentity>(getTarget(addItem, null));
        }

        foreach (var combinePair in mergeResult.CombineItems)
        {
            yield return new SaveItemData<TTarget, TIdentity>(getTarget(combinePair.Item2, combinePair.Item1));
        }
    }

    public static void Compress<TItem, TIdentity>(this ICollection<UpdateItemData<TItem, TIdentity>> items)
            where TItem : IUpdateDTO
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        items.RemoveBy(item =>
                       {
                           if (item is SaveItemData<TItem, TIdentity>)
                           {
                               var saveItem = item as SaveItemData<TItem, TIdentity>;

                               saveItem.Value.Compress();

                               return saveItem.Value.IsEmpty;
                           }

                           return false;
                       });
    }

    public static bool IsEmpty<TItem>(this Maybe<TItem> maybeItem)
            where TItem : IUpdateDTO
    {
        if (maybeItem == null) throw new ArgumentNullException(nameof(maybeItem));

        return maybeItem.Match(item =>
                               {
                                   item.Compress();
                                   return item.IsEmpty;
                               }, () => true);
    }


    public static Maybe<TItem> GetActualUpdateElement<TItem>(this Maybe<TItem> maybeItem)
            where TItem : IUpdateDTO
    {
        if (maybeItem == null) throw new ArgumentNullException(nameof(maybeItem));

        if (maybeItem.IsEmpty())
        {
            return Maybe<TItem>.Nothing;
        }
        else
        {
            return maybeItem;
        }
    }
}
