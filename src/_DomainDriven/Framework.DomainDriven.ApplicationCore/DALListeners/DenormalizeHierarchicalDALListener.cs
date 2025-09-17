using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;

using Framework.Core;
using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class DenormalizeHierarchicalDALListener(IServiceProvider serviceProvider) : IBeforeTransactionCompletedDALListener
{
    public void Process(DALChangesEventArgs eventArgs)
    {
        if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

        foreach (var typeGroup in eventArgs.Changes.GroupByType())
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

        where TDomainObject : class, IDenormalizedHierarchicalPersistentSource<TAncestorChildLink, TSourceToAncestorOrChildLink, TDomainObject, Guid>

        where TAncestorChildLink : class, IModifiedHierarchicalAncestorLink<TDomainObject, TSourceToAncestorOrChildLink, Guid>, new()

        where TSourceToAncestorOrChildLink : IHierarchicalToAncestorOrChildLink<TDomainObject, Guid>
    {
        var service = ActivatorUtilities
            .CreateInstance<
                SyncDenormalizedValuesService<TDomainObject, TAncestorChildLink,
                TSourceToAncestorOrChildLink, Guid>>(serviceProvider);

        service.Sync(modified, removing);
    }

    private static class DenormalizeCache
    {
        private static readonly MethodInfo DenormalizeMethod = typeof(DenormalizeHierarchicalDALListener).GetMethod(nameof(Denormalize), BindingFlags.NonPublic | BindingFlags.Instance, true);

        public static readonly IDictionaryCache<Type, MethodInfo> DenormalizeMethods = new DictionaryCache<Type, MethodInfo>(type =>
                type.GetInterfaceImplementationArguments(typeof(IDenormalizedHierarchicalPersistentSource<,,,>))
                    .Maybe(args => DenormalizeMethod.MakeGenericMethod(args[2], args[0], args[1]))).WithLock();
    }
}
