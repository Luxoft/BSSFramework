using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.HierarchicalExpand;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class AuthorizationExternalSource<TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode> : BLLContextContainer<TBLLContext>, IAuthorizationExternalSource
        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>, IBLLBaseContextBase<TPersistentDomainObjectBase, Guid>

        where TPersistentDomainObjectBase : class, IDefaultIdentityObject
        where TAuditPersistentDomainObjectBase : class, TPersistentDomainObjectBase, IDefaultAuditPersistentDomainObjectBase
{
    private readonly IAuthorizationBLLContext _authorizationBLLContext;

    private readonly ITypeResolver<string> _securityEntityTypeResolver;

    private readonly IDictionaryCache<EntityType, IAuthorizationTypedExternalSource> _typedCache;


    public AuthorizationExternalSource(TBLLContext context, IAuthorizationBLLContext authorizationBLLContext, ISecurityTypeResolverContainer securityTypeResolverContainer)
            : base (context)
    {
        if (authorizationBLLContext == null) throw new ArgumentNullException(nameof(authorizationBLLContext));
        if (securityTypeResolverContainer == null) throw new ArgumentNullException(nameof(securityTypeResolverContainer));

        this._authorizationBLLContext = authorizationBLLContext;
        this._securityEntityTypeResolver = securityTypeResolverContainer.SecurityTypeResolver;

        this._typedCache = new DictionaryCache<EntityType, IAuthorizationTypedExternalSource>(entityType => this.GetTypedInternal(entityType, true));
    }


    public Type SecurityOperationCodeType => typeof(TSecurityOperationCode);

    public IAuthorizationTypedExternalSource GetTyped(EntityType entityType, bool withCache = true)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        return withCache ? this._typedCache[entityType] : this.GetTypedInternal(entityType, false);
    }


    private IAuthorizationTypedExternalSource GetTypedInternal(EntityType entityType, bool withCache)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        var domainType = this._securityEntityTypeResolver.Resolve(entityType.Name, true);

        var authorizationTypedExternalSourceType = entityType.Expandable
                                                           ? typeof(HierarchicalAuthorizationTypedExternalSource<>)
                                                           : typeof(PlainAuthorizationTypedExternalSource<>);


        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(typeof(TBLLContext), typeof(TPersistentDomainObjectBase), typeof(TAuditPersistentDomainObjectBase), typeof(TSecurityOperationCode), domainType);

        var authorizationTypedExternalSourceImplTypeCtor = authorizationTypedExternalSourceImplType.GetConstructors().Single();

        var result = (IAuthorizationTypedExternalSource)authorizationTypedExternalSourceImplTypeCtor.Invoke(new object[] { this.Context, this._authorizationBLLContext, entityType, GetDisplayFunc(domainType) });

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
            throw new Exception($"{domainType.Name} must be implement {typeof (ISecurityVisualIdentityObject).Name} or {typeof (IVisualIdentityObject).Name}");
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



    private abstract class AuthorizationTypedExternalSourceBase<TDomainObject> : BLLContextContainer<TBLLContext>, IAuthorizationTypedExternalSource
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        private readonly IAuthorizationBLLContext _authorizationBLLContext;

        private readonly EntityType _entityType;
        private readonly Func<TDomainObject, string> _getSecurityNameFunc;


        protected AuthorizationTypedExternalSourceBase(TBLLContext context, IAuthorizationBLLContext authorizationBLLContext, EntityType entityType, Func<TDomainObject, string> getSecurityNameFunc)
                : base (context)
        {
            if (authorizationBLLContext == null) throw new ArgumentNullException(nameof(authorizationBLLContext));
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            if (getSecurityNameFunc == null) throw new ArgumentNullException(nameof(getSecurityNameFunc));

            this._authorizationBLLContext = authorizationBLLContext;
            this._entityType = entityType;
            this._getSecurityNameFunc = getSecurityNameFunc;
        }


        protected abstract Func<TDomainObject, Func<TDomainObject, string>, SecurityEntity> CreateSecurityEntityFunc { get; }

        private SecurityEntity CreateSecurityEntity(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.CreateSecurityEntityFunc(domainObject, this._getSecurityNameFunc);
        }


        public IEnumerable<SecurityEntity> GetSecurityEntities()
        {
            return this.Context.Logics.Default.Create<TDomainObject>().GetFullList().Select(this.CreateSecurityEntity);
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> preSecurityEntityIdents)
        {
            var securityEntityIdents = preSecurityEntityIdents.ToList();

            return this.Context.Logics.Default.Create<TDomainObject>().GetListByIdents(securityEntityIdents.ToArray()).Select(this.CreateSecurityEntity);
        }




        public IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId)
        {
            var secutityObject = this.Context.Logics.Default.Create<TDomainObject>().GetById(startSecurityEntityId, true);

            return this.GetSecurityEntitiesWithMasterExpand(secutityObject).Select(this.CreateSecurityEntity);
        }


        protected abstract IEnumerable<TDomainObject> GetSecurityEntitiesWithMasterExpand(TDomainObject startSecurityObject);


        public PermissionFilterEntity AddSecurityEntity(SecurityEntity securityEntity, bool disableExistsCheck = false)
        {
            if (securityEntity == null) throw new ArgumentNullException(nameof(securityEntity));

            var bll = this._authorizationBLLContext.Logics.PermissionFilterEntity;

            var existsFilterEntity = bll.GetObjectBy(v => v.EntityType == this._entityType && v.EntityId == securityEntity.Id);

            if (disableExistsCheck)
            {
                return existsFilterEntity ?? new PermissionFilterEntity
                                             {
                                                     EntityType = this._entityType,
                                                     EntityId = securityEntity.Id
                                             }.Self(bll.Save);
            }
            else
            {
                var entity = this.Context.Logics.Default.Create<TDomainObject>().GetById(securityEntity.Id, true);

                return existsFilterEntity ?? new PermissionFilterEntity
                                             {
                                                     EntityType = this._entityType,
                                                     EntityId = entity.Id
                                             }.Self(bll.Save);
            }
        }
    }


    private class PlainAuthorizationTypedExternalSource<TDomainObject> : AuthorizationTypedExternalSourceBase<TDomainObject>
            where TDomainObject : class, TAuditPersistentDomainObjectBase
    {
        public PlainAuthorizationTypedExternalSource(TBLLContext context, IAuthorizationBLLContext authorizationBLLContext, EntityType entityType, Func<TDomainObject, string> getSecurityNameFunc)
                : base(context, authorizationBLLContext, entityType, getSecurityNameFunc)
        {

        }


        protected override Func<TDomainObject, Func<TDomainObject, string>, SecurityEntity> CreateSecurityEntityFunc => SecurityEntity.CreatePlain;

        protected override IEnumerable<TDomainObject> GetSecurityEntitiesWithMasterExpand(TDomainObject startSecurityObject)
        {
            return new[] { startSecurityObject };
        }
    }


    private class HierarchicalAuthorizationTypedExternalSource<TDomainObject> : AuthorizationTypedExternalSourceBase<TDomainObject>
            where TDomainObject : class, TAuditPersistentDomainObjectBase, IDefaultHierarchicalPersistentDomainObjectBase<TDomainObject>
    {
        public HierarchicalAuthorizationTypedExternalSource(TBLLContext context, IAuthorizationBLLContext authorizationBLLContext, EntityType entityType, Func<TDomainObject, string> getSecurityNameFunc)
                : base(context, authorizationBLLContext, entityType, getSecurityNameFunc)
        {

        }


        protected override Func<TDomainObject, Func<TDomainObject, string>, SecurityEntity> CreateSecurityEntityFunc => SecurityEntity.CreateHierarchical;

        protected override IEnumerable<TDomainObject> GetSecurityEntitiesWithMasterExpand(TDomainObject startSecurityObject)
        {
            return startSecurityObject.GetAllParents();
        }
    }
}
