using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.Core;
using Framework.Persistent;


using JetBrains.Annotations;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Неконтекстный провайдер доступа
    /// </summary>
    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    public class NonContextSecurityProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : FixedSecurityProvider<TDomainObject>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase

        where TSecurityOperationCode : struct, Enum
    {
        private readonly NonContextSecurityOperation<TSecurityOperationCode> securityOperation;

        private readonly IAuthorizationSystem<TIdent> authorizationSystem;

        public NonContextSecurityProvider(
            NonContextSecurityOperation<TSecurityOperationCode> securityOperation,
            IAuthorizationSystem<TIdent> authorizationSystem)
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));
            if (authorizationSystem == null) throw new ArgumentNullException(nameof(authorizationSystem));
            if (securityOperation.Code.IsDefault()) throw new ArgumentOutOfRangeException(nameof(securityOperation));

            this.securityOperation = securityOperation;
            this.authorizationSystem = authorizationSystem;
        }


        protected override bool HasAccess()
        {
            return this.authorizationSystem.HasAccess(this.securityOperation);
        }


        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            return this.authorizationSystem.GetAccessors(this.securityOperation.Code, principal => principal.Permissions.Any())
                                      .ToUnboundedList();
        }
    }
}
