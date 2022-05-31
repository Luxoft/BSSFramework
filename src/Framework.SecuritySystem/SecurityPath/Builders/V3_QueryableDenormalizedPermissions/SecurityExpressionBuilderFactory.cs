using System;

using Framework.HierarchicalExpand;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.QueryableDenormalizedPermissions
{
    public class SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : SecurityExpressionBuilderFactoryBase<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        public SecurityExpressionBuilderFactory([NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory, [NotNull] IAuthorizationSystem<TIdent> authorizationSystem)
        {
            this.HierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory ?? throw new ArgumentNullException(nameof(hierarchicalObjectExpanderFactory));
            this.AuthorizationSystem = authorizationSystem;
        }

        public IHierarchicalObjectExpanderFactory<TIdent> HierarchicalObjectExpanderFactory { get; }

        public IAuthorizationSystem<TIdent> AuthorizationSystem { get; }

        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath securityPath)
        {
            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath>
                  .ConditionBinarySecurityPathExpressionBuilder(this, securityPath);
        }

        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext> securityPath)
        {
            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>>
                  .SecurityByIdentsExpressionBuilder<TSecurityContext>(this, securityPath);
        }

        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ManySecurityPath<TSecurityContext> securityPath)
        {
            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>>
                  .ManySecurityExpressionBuilder<TSecurityContext>(this, securityPath);
        }

        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SingleSecurityPath<TSecurityContext> securityPath)
        {
            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>>
                  .SingleSecurityExpressionBuilder<TSecurityContext>(this, securityPath);
        }

        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath securityPath)
        {
            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath>
                  .OrBinarySecurityPathExpressionBuilder(this, securityPath);
        }

        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath securityPath)
        {
            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath>
                  .AndBinarySecurityPathExpressionBuilder(this, securityPath);
        }

        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TNestedObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TNestedObject> securityPath)
        {
            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TNestedObject>>
                  .NestedManySecurityExpressionBuilder<TNestedObject>(this, securityPath);
        }
    }
}
