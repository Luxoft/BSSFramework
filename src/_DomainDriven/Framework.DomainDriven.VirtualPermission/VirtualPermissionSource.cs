using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.UserSource;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSource<TDomainObject>(
    ISecurityContextSource securityContextSource,
    ICurrentUser currentUser,
    IQueryableSource queryableSource,
    VirtualPermissionBindingInfo<TDomainObject> bindingInfo) : IPermissionSource
{
    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = this.GetPermissionQuery().ToList();

        return permissions.Select(this.ConvertPermission).ToList();
    }

    public IQueryable<IPermission> GetPermissionQuery(bool applyCurrentUser) =>
        this.GetPermissionQuery().Pipe(applyCurrentUser, q => q.Where(p => p.PrincipalName == currentUser.Name));

    public IQueryable<IPermission> GetPermissionQuery()
    {
        var baseQueryable = queryableSource.GetQueryable<TDomainObject>().Where(bindingInfo.Filter);

        return baseQueryable.Select(domainObject => new VirtualPermission<TDomainObject>(domainObject, bindingInfo));
    }

    public IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter) =>
        this.GetPermissionQuery().Where(permissionFilter).Select(p => p.PrincipalName);

    private Dictionary<Type, List<Guid>> ConvertPermission(IPermission permission) =>
        permission
            .Restrictions
            .GroupBy(p => p.SecurityContextTypeId)
            .Select(g => (securityContextSource.GetSecurityContextInfo(g.Key).Type, g.Select(v => v.SecurityContextId).ToList()))
            .ToDictionary();
}

public class VirtualPermission<TDomainObject>(
    TDomainObject domainObject,
    VirtualPermissionBindingInfo<TDomainObject> bindingInfo) : IPermission
{
    public IEnumerable<IPermissionRestriction> Restrictions { get; } =
        bindingInfo.SecurityContextPaths.SelectMany(v => CreateRestriction(v, domainObject));

    public string PrincipalName { get; } = bindingInfo.PrincipalNamePath.Eval(domainObject);

    private static IEnumerable<IPermissionRestriction> CreateRestriction(LambdaExpression securityContextPath, TDomainObject domainObject)
    {
    }

    private static IEnumerable<IPermissionRestriction> CreateRestriction<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> securityContextPath,
        TDomainObject domainObject)
    {
    }

    private static IEnumerable<IPermissionRestriction> CreateRestriction<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityContextPath,
        TDomainObject domainObject)
    {
        return securityContextPath.Eval(domainObject).Select()
    }
}

public record VirtualPermissionRestriction(Guid SecurityContextTypeId, Guid SecurityContextId) : IPermissionRestriction;

public class VirtualPermissionRestrictionFactory<TDomainObject>(
    TDomainObject domainObject,
    VirtualPermissionBindingInfo<TDomainObject> bindingInfo) : IPermission
{
    private static IEnumerable<IPermissionRestriction> CreateRestriction(LambdaExpression securityContextPath, TDomainObject domainObject)
    {
    }

    private static IEnumerable<IPermissionRestriction> CreateRestriction<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> securityContextPath,
        TDomainObject domainObject)
    {
    }

    private static IEnumerable<IPermissionRestriction> CreateRestriction<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityContextPath,
        TDomainObject domainObject)
    {
        return securityContextPath.Eval(domainObject).Select()
    }
}
