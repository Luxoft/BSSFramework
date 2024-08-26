using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.UserSource;

namespace Framework.Authorization.Environment.Security;

public class PrincipalSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>

    where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>
{
    private readonly ICurrentUser currentUser;

    private readonly Lazy<Expression<Func<TDomainObject, bool>>> lazySecurityFilter;


    public PrincipalSecurityProvider(ICurrentUser currentUser, IRelativeDomainPathInfo<TDomainObject, Principal> toPrincipalPathInfo)
    {
        this.currentUser = currentUser;

        this.lazySecurityFilter = LazyHelper.Create(
            () =>
            {
                Expression<Func<Principal, bool>> principalFilter = principal => principal.Name == this.currentUser.Name;

                return principalFilter.OverrideInput(toPrincipalPathInfo.Path);
            });
    }

    public override Expression<Func<TDomainObject, bool>> SecurityFilter => this.lazySecurityFilter.Value;

    protected override LambdaCompileMode SecurityFilterCompileMode { get; } = LambdaCompileMode.All;

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        SecurityAccessorData.Return(this.currentUser.Name);
}
