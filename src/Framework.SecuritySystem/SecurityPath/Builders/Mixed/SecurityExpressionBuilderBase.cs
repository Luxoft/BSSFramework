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
        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> hasAccessBuilder;

        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> queryBuilder;

        public SecurityExpressionBuilder(
                [NotNull] ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> hasAccessBuilder,
                [NotNull] ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> queryBuilder)
        {
            this.hasAccessBuilder = hasAccessBuilder ?? throw new ArgumentNullException(nameof(hasAccessBuilder));
            this.queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
        }

        public ISecurityExpressionFilter<TDomainObject> GetFilter<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
                where TSecurityOperationCode : struct, Enum
        {
            var hasAccessFilter = this.hasAccessBuilder.GetFilter(securityOperation);
            var queryFilter = this.queryBuilder.GetFilter(securityOperation);

            return new SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TIdent>(hasAccessFilter, queryFilter);
        }
    }
}
