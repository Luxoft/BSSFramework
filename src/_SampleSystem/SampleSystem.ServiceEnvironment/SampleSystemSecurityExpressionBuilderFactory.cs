using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemSecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : Framework.SecuritySystem.Rules.Builders.Mixed.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public SampleSystemSecurityExpressionBuilderFactory([NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory, [NotNull] IAuthorizationSystem<TIdent> authorizationSystem)
            : base(
                   new Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(hierarchicalObjectExpanderFactory, authorizationSystem),
                   new Framework.SecuritySystem.Rules.Builders.QueryablePermissions.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(hierarchicalObjectExpanderFactory, authorizationSystem))
    {
    }
}
