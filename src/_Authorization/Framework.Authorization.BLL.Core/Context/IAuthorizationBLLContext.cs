using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLL.Tracking;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL
{
    public partial interface IAuthorizationBLLContext :

        IAuthorizationBLLContext<Guid>,

        ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>,

        ITrackingServiceContainer<PersistentDomainObjectBase>,

        ITypeResolverContainer<string>,

        IConfigurationBLLContextContainer<IConfigurationBLLContext>,

        IDateTimeServiceContainer
    {
        IAuthorizationExternalSource ExternalSource { get; }

        Principal CurrentPrincipal { get; }

        Settings Settings { get; }


        ISecurityProvider<TDomainObject> GetPrincipalSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, Principal>> principalSecurityPath)
            where TDomainObject : PersistentDomainObjectBase;

        ISecurityProvider<TDomainObject> GetBusinessRoleSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, BusinessRole>> businessRoleSecurityPath)
            where TDomainObject : PersistentDomainObjectBase;

        ISecurityProvider<Operation> GetOperationSecurityProvider();


        bool HasAccess(Operation operation);

        IEnumerable<string> GetAccessors(Operation operation, Expression<Func<Principal, bool>> principalFilter);


        ITypeResolver<EntityType> SecurityTypeResolver { get; }


        EntityType GetEntityType(Type type);

        EntityType GetEntityType(string domainTypeName);

        EntityType GetEntityType(Guid domainTypeId);


        /// <summary>
        /// Получение форматированного вида пермиссии
        /// </summary>
        /// <param name="permission">Пермиссия</param>
        /// <param name="separator">Разделитель</param>
        /// <returns></returns>
        string GetFormattedPermission(Permission permission, string separator = " | ");
    }
}
