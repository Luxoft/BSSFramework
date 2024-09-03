using System.Collections.ObjectModel;

using Framework.Core;

namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public static class TypedSecurityEntitySourceExtensions
{
    public static ITypedSecurityEntitySource WithCache(this ITypedSecurityEntitySource source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new TypedSecurityEntitySource(source);
    }

    private class TypedSecurityEntitySource(ITypedSecurityEntitySource baseSource) : ITypedSecurityEntitySource
    {
        private readonly Lazy<ReadOnlyCollection<SecurityEntity>> lazySecurityEntities = LazyHelper.Create(() => baseSource.GetSecurityEntities().ToReadOnlyCollection());

        private readonly IDictionaryCache<Guid[], SecurityEntity[]> securityEntitiesByIdentsCache = new DictionaryCache<Guid[], SecurityEntity[]>(
                securityEntityIdents => baseSource.GetSecurityEntitiesByIdents(securityEntityIdents).ToArray(),
                ArrayComparer<Guid>.Value);

        private readonly IDictionaryCache<Guid, SecurityEntity[]> securityEntitiesWithMasterExpandCache = new DictionaryCache<Guid, SecurityEntity[]>(
                startSecurityEntityId => baseSource.GetSecurityEntitiesWithMasterExpand(startSecurityEntityId).ToArray());

        private readonly IDictionaryCache<Guid, bool> existsCache = new DictionaryCache<Guid, bool>(baseSource.IsExists);

        public IEnumerable<SecurityEntity> GetSecurityEntities()
        {
            return this.lazySecurityEntities.Value;
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> securityEntityIdents)
        {
            return this.securityEntitiesByIdentsCache[securityEntityIdents.ToArray()];
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId)
        {
            return this.securityEntitiesWithMasterExpandCache[startSecurityEntityId];
        }

        public bool IsExists(Guid securityEntityId)
        {
            return this.existsCache[securityEntityId];
        }
    }
}
