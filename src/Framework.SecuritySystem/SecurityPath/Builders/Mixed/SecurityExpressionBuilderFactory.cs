using System;

using Framework.HierarchicalExpand;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.Mixed
{
    public class SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> firstFactory;

        private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> secondFactory;

        public SecurityExpressionBuilderFactory(
                [NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
                [NotNull] IAuthorizationSystem<TIdent> authorizationSystem,
                [NotNull] ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> firstFactory,
                [NotNull] ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> secondFactory
                )
        {
            this.firstFactory = firstFactory ?? throw new ArgumentNullException(nameof(firstFactory));
            this.secondFactory = secondFactory ?? throw new ArgumentNullException(nameof(secondFactory));
        }

        public ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> path)
                where TDomainObject : class, TPersistentDomainObjectBase
        {
            var b1 = this.firstFactory.CreateBuilder(path);
            var b2 = this.secondFactory.CreateBuilder(path);

            return new SecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>(b1, b2);
        }
    }
}
