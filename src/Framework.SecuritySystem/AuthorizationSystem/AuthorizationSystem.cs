using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

using JetBrains.Annotations;

namespace Framework.SecuritySystem;

public abstract class AuthorizationSystem<TIdent> : IAuthorizationSystem<TIdent>
{
    private readonly IPrincipalPermissionSource<TIdent> principalPermissionSource;

    private readonly IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory;


    protected AuthorizationSystem(IPrincipalPermissionSource<TIdent> principalPermissionSource, IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory)
    {
        this.principalPermissionSource = principalPermissionSource;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
    }


    public abstract TIdent ResolveSecurityTypeId(Type type);

    public abstract bool HasAccess<TSecurityOperationCode>(NonContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityOperationCode : struct, Enum;

    public string ResolveSecurityTypeName([NotNull] Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Name;
    }

    public abstract TIdent GrandAccessIdent { get; }

    public abstract IEnumerable<string> GetAccessors<TSecurityOperationCode>(
            TSecurityOperationCode securityOperationCode,
            Expression<Func<IPrincipal<TIdent>, bool>> principalFilter)
            where TSecurityOperationCode : struct, Enum;

    public List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation, IEnumerable<Type> securityTypes)
            where TSecurityOperationCode : struct, Enum
    {
        return this.principalPermissionSource.GetPermissions().ToList(permission => this.TryExpandPermission(permission, securityOperation.SecurityExpandType));
    }

    public IQueryable<IPermission<TIdent>> GetPermissionQuery<TSecurityOperationCode>(
            ContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityOperationCode : struct, Enum
    {
        return this.principalPermissionSource.GetPermissionQuery(securityOperation);
    }

    private Dictionary<Type, IEnumerable<TIdent>> TryExpandPermission(Dictionary<Type, List<TIdent>> permission, HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
                                       pair => pair.Key,
                                       pair => this.hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }
}
