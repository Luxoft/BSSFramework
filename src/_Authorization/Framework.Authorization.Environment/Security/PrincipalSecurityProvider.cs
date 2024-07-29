using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public class PrincipalSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>

    where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>
{
    private readonly IActualPrincipalSource actualPrincipalSource;

    private readonly Lazy<Expression<Func<TDomainObject, bool>>> lazySecurityFilter;


    public PrincipalSecurityProvider(
        IActualPrincipalSource actualPrincipalSource,
        IRelativeDomainPathInfo<TDomainObject, Principal> toPrincipalPathInfo)
    {
        this.actualPrincipalSource = actualPrincipalSource;

        this.lazySecurityFilter = LazyHelper.Create(
            () =>
            {
                var actualPrincipalName = this.actualPrincipalSource.ActualPrincipal.Name;

                Expression<Func<Principal, bool>> principalFilter = principal => principal.Name == actualPrincipalName;

                return principalFilter.OverrideInput(toPrincipalPathInfo.Path);
            });
    }

    public override Expression<Func<TDomainObject, bool>> SecurityFilter => this.lazySecurityFilter.Value;

    protected override LambdaCompileMode SecurityFilterCompileMode { get; } = LambdaCompileMode.All;

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        SecurityAccessorData.Return(this.actualPrincipalSource.ActualPrincipal.Name);
}
