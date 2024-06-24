using System.Collections.ObjectModel;

using Framework.Authorization.Domain;
using Framework.Core;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public static class AuthorizationTypedExternalSourceExtensions
{
    public static IAuthorizationTypedExternalSource WithCache(this IAuthorizationTypedExternalSource source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new AuthorizationTypedExternalSource(source);
    }

    private class AuthorizationTypedExternalSource(IAuthorizationTypedExternalSource baseSource) : IAuthorizationTypedExternalSource
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
