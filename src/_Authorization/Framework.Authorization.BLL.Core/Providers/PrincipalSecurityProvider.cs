using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL;

internal class PrincipalSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>

        where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>
{

    private readonly Lazy<Expression<Func<TDomainObject, bool>>> lazySecurityFilter;


    internal PrincipalSecurityProvider(IAuthorizationBLLContext context, Expression<Func<TDomainObject, Principal>> principalSecurityPath)
            : base (context.AccessDeniedExceptionService)
    {
        if (principalSecurityPath == null) throw new ArgumentNullException(nameof(principalSecurityPath));

        this.Context = context;

        this.lazySecurityFilter = LazyHelper.Create(() =>
                                                    {
                                                        var principalName = this.Context.RunAsManager.PrincipalName;

                                                        Expression<Func<Principal, bool>> principalFilter = principal => principal.Name == principalName;

                                                        return principalFilter.OverrideInput(principalSecurityPath);
                                                    });
    }
    public IAuthorizationBLLContext Context { get; }

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
        return UnboundedList.Yeild(this.Context.RunAsManager.PrincipalName);
    }
}
