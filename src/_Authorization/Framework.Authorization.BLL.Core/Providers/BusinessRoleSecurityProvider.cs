using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL;

internal class BusinessRoleSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>

        where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>
{
    public IAuthorizationBLLContext Context { get; }

    private readonly Expression<Func<TDomainObject, BusinessRole>> businessRoleSecurityPath;
    private readonly Lazy<Expression<Func<TDomainObject, bool>>> lazySecurityFilter;


    internal BusinessRoleSecurityProvider(IAuthorizationBLLContext context, Expression<Func<TDomainObject, BusinessRole>> businessRoleSecurityPath)
            :base(context.AccessDeniedExceptionService)
    {
        this.Context = context;
        this.businessRoleSecurityPath = businessRoleSecurityPath;
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (businessRoleSecurityPath == null) throw new ArgumentNullException(nameof(businessRoleSecurityPath));

        this.lazySecurityFilter = LazyHelper.Create(() =>
                                                    {
                                                        var principalName = this.Context.RunAsManager.PrincipalName;

                                                        var today = context.DateTimeService.Today;

                                                        Expression<Func<BusinessRole, bool>> filter = businessRole =>

                                                                businessRole.Permissions.Any(permission => permission.Status == PermissionStatus.Approved
                                                                    && permission.Period.Contains(today)
                                                                    && permission.Principal.Name == principalName);

                                                        return filter.OverrideInput(this.businessRoleSecurityPath);
                                                    });
    }

    protected override LambdaCompileMode SecurityFilterCompileMode
    {
        get { return LambdaCompileMode.All; }
    }


    public override Expression<Func<TDomainObject, bool>> SecurityFilter
    {
        get { return this.lazySecurityFilter.Value; }
    }

    public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        var role = this.businessRoleSecurityPath.Eval(domainObject);

        var today = this.Context.DateTimeService.Today;

        return role.Permissions
                   .Where(permission => permission.Status == PermissionStatus.Approved && permission.Period.Contains(today))
                   .Select(permission => permission.Principal)
                   .Distinct()
                   .Select(principal => principal.Name)
                   .ToUnboundedList();
    }
}
