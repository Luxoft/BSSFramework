using System;

using Framework.HierarchicalExpand;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.Mixed
{
    public class SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> v1Factory;

        private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> v2Factory;

        public SecurityExpressionBuilderFactory([NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory, [NotNull] IAuthorizationSystem<TIdent> authorizationSystem)
        {
            this.v1Factory = new V1.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(hierarchicalObjectExpanderFactory, authorizationSystem);
            this.v2Factory = new V2.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(hierarchicalObjectExpanderFactory, authorizationSystem);
        }

        public ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> path)
                where TDomainObject : class, TPersistentDomainObjectBase
        {
            var b1 = this.v1Factory.CreateBuilder(path);
            var b2 = this.v2Factory.CreateBuilder(path);

            return new SecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>(b1, b2);
        }
    }
}
