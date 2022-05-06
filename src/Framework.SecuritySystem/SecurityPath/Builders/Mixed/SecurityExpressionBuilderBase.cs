using System;

using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.Mixed
{
    public class SecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>
        : ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase

    {
        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> v1Builder;

        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> v2Builder;

        public SecurityExpressionBuilder(
                [NotNull] ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> v1Builder,
                [NotNull] ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> v2Builder)
        {
            this.v1Builder = v1Builder ?? throw new ArgumentNullException(nameof(v1Builder));
            this.v2Builder = v2Builder ?? throw new ArgumentNullException(nameof(v2Builder));
        }

        public ISecurityExpressionFilter<TDomainObject> GetFilter<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
                where TSecurityOperationCode : struct, Enum
        {
            var v1Filter = this.v1Builder.GetFilter(securityOperation);
            var v2Filter = this.v2Builder.GetFilter(securityOperation);

            return new SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode, TIdent>(v1Filter, v2Filter);
        }
    }
}
