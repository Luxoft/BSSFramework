using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Неконтекстный провайдер доступа
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    public class NonContextSecurityProvider<TDomainObject, TIdent> : FixedSecurityProvider<TDomainObject>, ISecurityProvider<TDomainObject>
        where TDomainObject : class, IIdentityObject<TIdent>
    {
        private readonly NonContextSecurityOperation securityOperation;

        private readonly IAuthorizationSystem<TIdent> authorizationSystem;

        public NonContextSecurityProvider(
            NonContextSecurityOperation securityOperation,
            IAuthorizationSystem<TIdent> authorizationSystem)
        {
            this.securityOperation = securityOperation ?? throw new ArgumentNullException(nameof(securityOperation));
            this.authorizationSystem = authorizationSystem ?? throw new ArgumentNullException(nameof(authorizationSystem));
        }


        protected override bool HasAccess()
        {
            return this.authorizationSystem.HasAccess(this.securityOperation);
        }

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            if (this.HasAccess(domainObject))
            {
                return AccessResult.AccessGrantedResult.Default;
            }
            else
            {
                return new AccessResult.AccessDeniedResult
                       {
                           SecurityOperation = this.securityOperation, DomainObjectInfo = (domainObject, typeof(TDomainObject))
                       };
            }
        }


        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            return this.authorizationSystem.GetAccessors(this.securityOperation, principal => principal.Permissions.Any())
                                      .ToUnboundedList();
        }
    }
}
