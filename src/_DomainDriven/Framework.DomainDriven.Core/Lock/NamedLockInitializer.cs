using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.GenericQueryable;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.Lock;

public class NamedLockInitializer<TGenericNamedLock>(
    [DisabledSecurity] IRepository<TGenericNamedLock> namedLockRepository,
    INamedLockSource namedLockSource,
    GenericNamedLockTypeInfo<TGenericNamedLock> genericNamedLockTypeInfo)
    : INamedLockInitializer
    where TGenericNamedLock : new()
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var getNameFunc = genericNamedLockTypeInfo.NamePath.Compile(LambdaCompileCache.Default);
        var setNameFunc = genericNamedLockTypeInfo.NamePath.ToSetLambdaExpression().Compile(LambdaCompileCache.Default);

        var dbValues = await namedLockRepository.GetQueryable().ToGenericListAsync(cancellationToken);

        var mergeResult = dbValues.GetMergeResult(namedLockSource.NamedLocks, getNameFunc, v => v.Name);

        foreach (var addingItem in mergeResult.AddingItems)
        {
            await namedLockRepository.SaveAsync(new TGenericNamedLock().Self(v => setNameFunc(v, addingItem.Name)), cancellationToken);
        }

        foreach (var removingItem in mergeResult.RemovingItems)
        {
            await namedLockRepository.RemoveAsync(removingItem, cancellationToken);
        }
    }
}
