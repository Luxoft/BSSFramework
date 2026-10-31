using Anch.Core;
using Anch.GenericQueryable;
using Anch.SecuritySystem.Attributes;

using Framework.Application.Repository;
using Framework.Core;

namespace Framework.Application.Lock;

public class NamedLockInitializer<TGenericNamedLock>(
    [DisabledSecurity] IRepository<TGenericNamedLock> namedLockRepository,
    INamedLockSource namedLockSource,
    GenericNamedLockTypeInfo<TGenericNamedLock> genericNamedLockTypeInfo)
    : INamedLockInitializer
    where TGenericNamedLock : new()
{
    /// <summary>
    /// Synchronizes the lock table with <see cref="INamedLockSource"/> by name: missing locks are inserted, locks that are no longer
    /// declared are DELETED. Not safe to run concurrently from several application instances (both may try to insert the same name).
    /// </summary>
    public async Task Initialize(CancellationToken ct)
    {
        var getNameFunc = genericNamedLockTypeInfo.NamePath.Compile();
        var setNameFunc = genericNamedLockTypeInfo.NamePath.ToSetLambdaExpression().Compile();

        var dbValues = await namedLockRepository.GetQueryable().GenericToListAsync(ct);

        var mergeResult = dbValues.GetMergeResult(namedLockSource.NamedLocks, getNameFunc, v => v.Name);

        foreach (var addingItem in mergeResult.AddingItems)
        {
            await namedLockRepository.SaveAsync(new TGenericNamedLock().Self(v => setNameFunc(v, addingItem.Name)), ct);
        }

        foreach (var removingItem in mergeResult.RemovingItems)
        {
            await namedLockRepository.RemoveAsync(removingItem, ct);
        }
    }
}
