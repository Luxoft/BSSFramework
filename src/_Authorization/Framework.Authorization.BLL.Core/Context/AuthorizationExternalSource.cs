using Framework.Core;
using Framework.Persistent;

using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.BLL;

public class AuthorizationExternalSource : IAuthorizationExternalSource
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    private readonly IDictionaryCache<EntityType, IAuthorizationTypedExternalSource> _typedCache;


    public AuthorizationExternalSource(IAuthorizationBLLContext authorizationBLLContext)
    {
        this.authorizationBllContext = authorizationBLLContext ?? throw new ArgumentNullException(nameof(authorizationBLLContext));

        this._typedCache = new DictionaryCache<EntityType, IAuthorizationTypedExternalSource>(entityType => this.GetTypedInternal(entityType, true));
    }

    public IAuthorizationTypedExternalSource GetTyped(EntityType entityType, bool withCache = true)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        return withCache ? this._typedCache[entityType] : this.GetTypedInternal(entityType, false);
    }

    private IAuthorizationTypedExternalSource GetTypedInternal(EntityType entityType, bool withCache)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        var domainType = this.authorizationBllContext.SecurityTypeResolver.Resolve(entityType, true);

        var authorizationTypedExternalSourceType = entityType.Expandable
                                                           ? typeof(HierarchicalAuthorizationTypedExternalSource<>)
                                                           : typeof(PlainAuthorizationTypedExternalSource<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(domainType);

        var result = (IAuthorizationTypedExternalSource)
            ActivatorUtilities.CreateInstance(this.authorizationBllContext.ServiceProvider, authorizationTypedExternalSourceImplType, entityType, GetDisplayFunc(domainType));

        return withCache ? result.WithCache() : result;
    }

    private static object GetDisplayFunc(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (typeof(ISecurityVisualIdentityObject).IsAssignableFrom(domainType))
        {
            return new Func<Func<ISecurityVisualIdentityObject, string>>(GetNewDisplayFunc<ISecurityVisualIdentityObject>)
                   .Method
                   .GetGenericMethodDefinition()
                   .MakeGenericMethod(domainType)
                   .Invoke(null, null);
        }
        else if (typeof(IVisualIdentityObject).IsAssignableFrom(domainType))
        {
            return new Func<Func<IVisualIdentityObject, string>>(GetOldDisplayFunc<IVisualIdentityObject>)
                   .Method
                   .GetGenericMethodDefinition()
                   .MakeGenericMethod(domainType)
                   .Invoke(null, null);
        }
        else
        {
            throw new Exception($"{domainType.Name} must be implement {nameof(ISecurityVisualIdentityObject)} or {nameof(IVisualIdentityObject)}");
        }
    }

    private static Func<TDomainObject, string> GetNewDisplayFunc<TDomainObject>()
            where TDomainObject : ISecurityVisualIdentityObject
    {
        return domainObject => domainObject.Name;
    }

    private static Func<TDomainObject, string> GetOldDisplayFunc<TDomainObject>()
            where TDomainObject : IVisualIdentityObject
    {
        return domainObject => domainObject.Name;
    }



    private abstract class AuthorizationTypedExternalSourceBase<TDomainObject> : IAuthorizationTypedExternalSource
            where TDomainObject : class, IIdentityObject<Guid>
    {
        private readonly IRepository<TDomainObject> domainRepository;

        private readonly IAuthorizationBLLContext authorizationBLLContext;

        private readonly EntityType entityType;

        private readonly Func<TDomainObject, string> getSecurityNameFunc;

        protected AuthorizationTypedExternalSourceBase(IRepository<TDomainObject> domainRepository, IAuthorizationBLLContext authorizationBLLContext, EntityType entityType, Func<TDomainObject, string> getSecurityNameFunc)
        {
            this.domainRepository = domainRepository;
            this.authorizationBLLContext = authorizationBLLContext ?? throw new ArgumentNullException(nameof(authorizationBLLContext));
            this.entityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            this.getSecurityNameFunc = getSecurityNameFunc ?? throw new ArgumentNullException(nameof(getSecurityNameFunc));
        }

        protected abstract Func<TDomainObject, Func<TDomainObject, string>, SecurityEntity> CreateSecurityEntityFunc { get; }

        private SecurityEntity CreateSecurityEntity(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.CreateSecurityEntityFunc(domainObject, this.getSecurityNameFunc);
        }

        public IEnumerable<SecurityEntity> GetSecurityEntities()
        {
            return this.domainRepository.GetQueryable().ToList().Select(this.CreateSecurityEntity);
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> preSecurityEntityIdents)
        {
            var securityEntityIdents = preSecurityEntityIdents.ToList();

            return this.domainRepository.GetQueryable().Where(obj => securityEntityIdents.Contains(obj.Id)).ToList()
                       .Select(this.CreateSecurityEntity);
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId)
        {
            var securityObject = this.domainRepository.GetQueryable().Single(obj => obj.Id == startSecurityEntityId);

            return this.GetSecurityEntitiesWithMasterExpand(securityObject).Select(this.CreateSecurityEntity);
        }

        protected abstract IEnumerable<TDomainObject> GetSecurityEntitiesWithMasterExpand(TDomainObject startSecurityObject);

        public PermissionFilterEntity AddSecurityEntity(SecurityEntity securityEntity, bool disableExistsCheck = false)
        {
            if (securityEntity == null) throw new ArgumentNullException(nameof(securityEntity));

            var bll = this.authorizationBLLContext.Logics.PermissionFilterEntity;

            var existsFilterEntity = bll.GetObjectBy(v => v.EntityType == this.entityType && v.EntityId == securityEntity.Id);

            if (disableExistsCheck)
            {
                return existsFilterEntity ?? new PermissionFilterEntity
                                             {
                                                     EntityType = this.entityType,
                                                     EntityId = securityEntity.Id
                                             }.Self(bll.Save);
            }
            else
            {
                var entity = this.domainRepository.GetQueryable().Single(obj => obj.Id == securityEntity.Id);

                return existsFilterEntity ?? new PermissionFilterEntity
                                             {
                                                     EntityType = this.entityType,
                                                     EntityId = entity.Id
                                             }.Self(bll.Save);
            }
        }
    }

    private class PlainAuthorizationTypedExternalSource<TDomainObject> : AuthorizationTypedExternalSourceBase<TDomainObject>
            where TDomainObject : class, IIdentityObject<Guid>, IActiveObject
    {
        public PlainAuthorizationTypedExternalSource(
            IRepository<TDomainObject> domainRepository,
            IAuthorizationBLLContext authorizationBLLContext,
            EntityType entityType,
            Func<TDomainObject, string> getSecurityNameFunc)
            : base(domainRepository, authorizationBLLContext, entityType, getSecurityNameFunc)
        {
        }

        protected override Func<TDomainObject, Func<TDomainObject, string>, SecurityEntity> CreateSecurityEntityFunc => SecurityEntity.CreatePlain;

        protected override IEnumerable<TDomainObject> GetSecurityEntitiesWithMasterExpand(TDomainObject startSecurityObject)
        {
            return new[] { startSecurityObject };
        }
    }

    private class HierarchicalAuthorizationTypedExternalSource<TDomainObject> : AuthorizationTypedExternalSourceBase<TDomainObject>
            where TDomainObject : class, IIdentityObject<Guid>, IActiveObject, IParentSource<TDomainObject>
    {
        public HierarchicalAuthorizationTypedExternalSource(
            IRepository<TDomainObject> domainRepository,
            IAuthorizationBLLContext authorizationBLLContext,
            EntityType entityType,
            Func<TDomainObject, string> getSecurityNameFunc)
            : base(domainRepository, authorizationBLLContext, entityType, getSecurityNameFunc)
        {
        }

        protected override Func<TDomainObject, Func<TDomainObject, string>, SecurityEntity> CreateSecurityEntityFunc => SecurityEntity.CreateHierarchical;

        protected override IEnumerable<TDomainObject> GetSecurityEntitiesWithMasterExpand(TDomainObject startSecurityObject)
        {
            return startSecurityObject.GetAllParents();
        }
    }
}
