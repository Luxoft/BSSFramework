using System.Collections.ObjectModel;

using Framework.Core;

namespace Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

public static class TypedSecurityContextStorageExtensions
{
    public static ITypedSecurityContextStorage WithCache(this ITypedSecurityContextStorage source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new TypedSecurityEntitySource(source);
    }

    private class TypedSecurityEntitySource(ITypedSecurityContextStorage baseSource) : ITypedSecurityContextStorage
    {
        private readonly Lazy<ReadOnlyCollection<SecurityContextData>> lazySecurityContexts = LazyHelper.Create(() => baseSource.GetSecurityContexts().ToReadOnlyCollection());

        private readonly IDictionaryCache<Guid[], SecurityContextData[]> securityEntitiesByIdentsCache = new DictionaryCache<Guid[], SecurityContextData[]>(
                securityEntityIdents => baseSource.GetSecurityContextsByIdents(securityEntityIdents).ToArray(),
                ArrayComparer<Guid>.Value);

        private readonly IDictionaryCache<Guid, SecurityContextData[]> securityEntitiesWithMasterExpandCache = new DictionaryCache<Guid, SecurityContextData[]>(
                startSecurityEntityId => baseSource.GetSecurityContextsWithMasterExpand(startSecurityEntityId).ToArray());

        private readonly IDictionaryCache<Guid, bool> existsCache = new DictionaryCache<Guid, bool>(baseSource.IsExists);

        public IEnumerable<SecurityContextData> GetSecurityContexts()
        {
            return this.lazySecurityContexts.Value;
        }

        public IEnumerable<SecurityContextData> GetSecurityContextsByIdents(IEnumerable<Guid> securityEntityIdents)
        {
            return this.securityEntitiesByIdentsCache[securityEntityIdents.ToArray()];
        }

        public IEnumerable<SecurityContextData> GetSecurityContextsWithMasterExpand(Guid startSecurityEntityId)
        {
            return this.securityEntitiesWithMasterExpandCache[startSecurityEntityId];
        }

        public bool IsExists(Guid securityEntityId)
        {
            return this.existsCache[securityEntityId];
        }
    }
}
