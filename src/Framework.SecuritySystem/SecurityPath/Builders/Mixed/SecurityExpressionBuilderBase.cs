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
        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> firstBuilder;

        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> secondBuilder;

        public SecurityExpressionBuilder(
                [NotNull] ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> v1Builder,
                [NotNull] ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> v2Builder)
        {
            this.firstBuilder = v1Builder ?? throw new ArgumentNullException(nameof(v1Builder));
            this.secondBuilder = v2Builder ?? throw new ArgumentNullException(nameof(v2Builder));
        }

        public ISecurityExpressionFilter<TDomainObject> GetFilter<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
                where TSecurityOperationCode : struct, Enum
        {
            var firstFilter = this.firstBuilder.GetFilter(securityOperation);
            var secondFilter = this.secondBuilder.GetFilter(securityOperation);

            return new SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TIdent>(firstFilter, secondFilter);
        }
    }
}
