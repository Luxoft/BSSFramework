using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public abstract class AuthorizationSystem<TIdent> : IAuthorizationSystem<TIdent>
{
    private readonly IPrincipalPermissionSource<TIdent> principalPermissionSource;

    private readonly IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory;


    protected AuthorizationSystem(
        IPrincipalPermissionSource<TIdent> principalPermissionSource,
        IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory)
    {
        this.principalPermissionSource = principalPermissionSource;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
    }


    public abstract TIdent ResolveSecurityTypeId(Type type);

    public abstract bool IsAdmin();

    public abstract bool HasAccess(NonContextSecurityOperation securityOperation);

    public abstract void CheckAccess(NonContextSecurityOperation securityOperation);

    public string ResolveSecurityTypeName(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Name;
    }

    public abstract IEnumerable<string> GetAccessors(
        NonContextSecurityOperation securityOperation,
        Expression<Func<IPrincipal<TIdent>, bool>> principalFilter);

    public List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        ContextSecurityOperation securityOperation,
        IEnumerable<Type> securityTypes)
    {
        return this.principalPermissionSource.GetPermissions()
                   .ToList(permission => this.TryExpandPermission(permission, securityOperation.ExpandType));
    }

    public IQueryable<IPermission<TIdent>> GetPermissionQuery(
        ContextSecurityOperation securityOperation)
    {
        return this.principalPermissionSource.GetPermissionQuery(securityOperation);
    }

    private Dictionary<Type, IEnumerable<TIdent>> TryExpandPermission(
        Dictionary<Type, List<TIdent>> permission,
        HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => this.hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }
}
