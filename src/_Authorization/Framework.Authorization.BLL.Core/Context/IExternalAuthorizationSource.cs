using System.Collections.ObjectModel;

using Framework.Authorization.Domain;
using Framework.Core;

namespace Framework.Authorization.BLL;

public interface IAuthorizationExternalSource
{
    Type SecurityOperationCodeType { get; }

    IAuthorizationTypedExternalSource GetTyped(EntityType entityType, bool withCache = true);
}


public interface IAuthorizationTypedExternalSourceBase
{
    IEnumerable<SecurityEntity> GetSecurityEntities();

    IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> securityEntityIdents);

    IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId);
}

public interface IAuthorizationTypedExternalSource : IAuthorizationTypedExternalSourceBase
{
    PermissionFilterEntity AddSecurityEntity(SecurityEntity securityEntity, bool disableExistsCheck = false);
}


public static class AuthorizationTypedExternalSourceExtensions
{
    public static IAuthorizationTypedExternalSource WithCache(this IAuthorizationTypedExternalSource source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new AuthorizationTypedExternalSource(source);
    }


    private class AuthorizationTypedExternalSource : IAuthorizationTypedExternalSource
    {
        private readonly IAuthorizationTypedExternalSource _baseSource;

        private readonly Lazy<ReadOnlyCollection<SecurityEntity>> _lazySecurityEntities;

        private readonly IDictionaryCache<Guid[], SecurityEntity[]> _securityEntitiesByIdentsCache;

        private readonly IDictionaryCache<Guid, SecurityEntity[]> _securityEntitiesWithMasterExpandCache;


        public AuthorizationTypedExternalSource(IAuthorizationTypedExternalSource baseSource)
        {
            if (baseSource == null) throw new ArgumentNullException(nameof(baseSource));

            this._baseSource = baseSource;

            this._lazySecurityEntities = LazyHelper.Create(() => this._baseSource.GetSecurityEntities().ToReadOnlyCollection());

            this._securityEntitiesByIdentsCache = new DictionaryCache<Guid[], SecurityEntity[]>(securityEntityIdents => this._baseSource.GetSecurityEntitiesByIdents(securityEntityIdents).ToArray(), ArrayComparer<Guid>.Value);

            this._securityEntitiesWithMasterExpandCache = new DictionaryCache<Guid, SecurityEntity[]>(startSecurityEntityId => this._baseSource.GetSecurityEntitiesWithMasterExpand(startSecurityEntityId).ToArray());
        }

        public IEnumerable<SecurityEntity> GetSecurityEntities()
        {
            return this._lazySecurityEntities.Value;
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> securityEntityIdents)
        {
            return this._securityEntitiesByIdentsCache[securityEntityIdents.ToArray()];
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId)
        {
            return this._securityEntitiesWithMasterExpandCache[startSecurityEntityId];
        }

        public PermissionFilterEntity AddSecurityEntity(SecurityEntity securityEntity, bool disableExistsCheck = false)
        {
            return this._baseSource.AddSecurityEntity(securityEntity, disableExistsCheck);
        }
    }
}
