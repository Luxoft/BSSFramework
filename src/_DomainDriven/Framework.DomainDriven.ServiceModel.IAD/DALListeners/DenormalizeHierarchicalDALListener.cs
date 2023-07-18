using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLL.Security.Lock;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class DenormalizeHierarchicalDALListener<TBLLContext, TPersistentDomainObjectBase, TNamedLockObject, TNamedLockOperation> : BLLContextContainer<TBLLContext>, IBeforeTransactionCompletedDALListener

        where TBLLContext : class, IBLLBaseContextBase<TPersistentDomainObjectBase, Guid>, ITrackingServiceContainer<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TNamedLockObject : class, TPersistentDomainObjectBase, INamedLock<TNamedLockOperation>
        where TNamedLockOperation : struct, Enum
{
    public DenormalizeHierarchicalDALListener([NotNull]TBLLContext context)
            : base(context)
    {
    }

    public void Process(DALChangesEventArgs eventArgs)
    {
        if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

        foreach (var typeGroup in eventArgs.Changes.GetSubset(typeof(TPersistentDomainObjectBase)).GroupByType())
        {
            DenormalizeCache.DenormalizeMethods[typeGroup.Key].Maybe(method =>
                                                                     {
                                                                         var values = typeGroup.Value.ToChangeTypeDict().Partial(pair => pair.Value == DALObjectChangeType.Created || pair.Value == DALObjectChangeType.Updated, (modified, removing) => new { Modified = modified, Removing = removing });

                                                                         method.Invoke(this, new object[] { values.Modified.Select(z => z.Key).ToArray(typeGroup.Key), values.Removing.Select(z => z.Key).ToArray(typeGroup.Key) });
                                                                     });
        }
    }

    private void Denormalize<TDomainObject, TAncestorChildLink, TSourceToAncestorOrChildLink>(
        TDomainObject[] modified,
        TDomainObject[] removing)

        where TDomainObject : class, TPersistentDomainObjectBase, IDenormalizedHierarchicalPersistentSource<TAncestorChildLink, TSourceToAncestorOrChildLink, TDomainObject, Guid>, IModifiedIHierarchicalLevelObject

        where TAncestorChildLink : class, TPersistentDomainObjectBase, IModifiedHierarchicalAncestorLink<TDomainObject, TSourceToAncestorOrChildLink, Guid>, new()

        where TSourceToAncestorOrChildLink : IHierarchicalToAncestorOrChildLink<TDomainObject, Guid>
    {
        var service = ActivatorUtilities
            .CreateInstance<
                SyncDenormolizedValuesService<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TAncestorChildLink,
                TSourceToAncestorOrChildLink, Guid, TNamedLockObject, TNamedLockOperation>>(this.Context.ServiceProvider);

        service.Sync(modified, removing);
    }

    private static class DenormalizeCache
    {
        private static readonly MethodInfo DenormalizeMethod = typeof(DenormalizeHierarchicalDALListener<TBLLContext, TPersistentDomainObjectBase, TNamedLockObject, TNamedLockOperation>).GetMethod(nameof(Denormalize), BindingFlags.NonPublic | BindingFlags.Instance, true);

        public static readonly IDictionaryCache<Type, MethodInfo> DenormalizeMethods = new DictionaryCache<Type, MethodInfo>(type =>
                type.GetInterfaceImplementationArguments(typeof(IDenormalizedHierarchicalPersistentSource<,,,>))
                    .Maybe(args => DenormalizeMethod.MakeGenericMethod(args[2], args[0], args[1]))).WithLock();
    }
}
