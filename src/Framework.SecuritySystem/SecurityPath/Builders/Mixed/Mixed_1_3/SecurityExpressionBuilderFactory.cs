using System;

using Framework.HierarchicalExpand;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.Mixed.V1_V3
{
    public class SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : Mixed.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>

            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        public SecurityExpressionBuilderFactory([NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory, [NotNull] IAuthorizationSystem<TIdent> authorizationSystem)
                : base(hierarchicalObjectExpanderFactory,
                       authorizationSystem,
                       new V1.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(hierarchicalObjectExpanderFactory, authorizationSystem),
                       new V3.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(hierarchicalObjectExpanderFactory, authorizationSystem))
        {
        }
    }
}
