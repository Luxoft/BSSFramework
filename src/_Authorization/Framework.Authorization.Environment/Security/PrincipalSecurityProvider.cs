using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment
{
    public class PrincipalSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>

        where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>
    {
        private readonly IRunAsManager runAsManager;

        private readonly Lazy<Expression<Func<TDomainObject, bool>>> lazySecurityFilter;


        public PrincipalSecurityProvider(IRunAsManager runAsManager, Expression<Func<TDomainObject, Principal>> principalSecurityPath)
        {
            this.runAsManager = runAsManager;

            this.lazySecurityFilter = LazyHelper.Create(
                () =>
                {
                    var actualPrincipalName = this.runAsManager.ActualPrincipal.Name;

                    Expression<Func<Principal, bool>> principalFilter = principal => principal.Name == actualPrincipalName;

                    return principalFilter.OverrideInput(principalSecurityPath);
                });
        }

        protected override LambdaCompileMode SecurityFilterCompileMode { get { return LambdaCompileMode.All; } }


        public override Expression<Func<TDomainObject, bool>> SecurityFilter { get { return this.lazySecurityFilter.Value; } }

        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            return UnboundedList.Yeild(this.runAsManager.ActualPrincipal.Name);
        }
    }
}
