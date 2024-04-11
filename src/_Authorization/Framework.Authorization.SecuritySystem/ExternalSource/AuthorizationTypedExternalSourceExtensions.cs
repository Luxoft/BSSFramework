﻿using System.Collections.ObjectModel;

using Framework.Authorization.Domain;
using Framework.Core;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public static class AuthorizationTypedExternalSourceExtensions
{
    public static IAuthorizationTypedExternalSourceBase WithCache(this IAuthorizationTypedExternalSourceBase source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new AuthorizationTypedExternalSource(source);
    }


    private class AuthorizationTypedExternalSource : IAuthorizationTypedExternalSourceBase
    {
        private readonly IAuthorizationTypedExternalSourceBase _baseSource;

        private readonly Lazy<ReadOnlyCollection<SecurityEntity>> _lazySecurityEntities;

        private readonly IDictionaryCache<Guid[], SecurityEntity[]> _securityEntitiesByIdentsCache;

        private readonly IDictionaryCache<Guid, SecurityEntity[]> _securityEntitiesWithMasterExpandCache;


        public AuthorizationTypedExternalSource(IAuthorizationTypedExternalSourceBase baseSource)
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
    }
}
