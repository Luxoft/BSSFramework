//using System;

//using Framework.HierarchicalExpand;
//using Framework.Persistent;

//

//namespace Framework.SecuritySystem.Rules.Builders.QueryableDenormalizedPermissions
//{
//    public class SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : SecurityExpressionBuilderFactoryBase<TPersistentDomainObjectBase, TIdent>

//        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
//    {
//        public SecurityExpressionBuilderFactory(IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory, IAuthorizationSystem<TIdent> authorizationSystem)
//        {
//            this.HierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory ?? throw new ArgumentNullException(nameof(hierarchicalObjectExpanderFactory));
//            this.AuthorizationSystem = authorizationSystem;
//        }

//        public IHierarchicalObjectExpanderFactory<TIdent> HierarchicalObjectExpanderFactory { get; }

//        public IAuthorizationSystem<TIdent> AuthorizationSystem { get; }

//        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.ConditionPath securityPath)
//        {
//            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.ConditionPath>
//                  .ConditionBinarySecurityPathExpressionBuilder(this, securityPath);
//        }

//        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.SecurityPathByIdents<TSecurityContext> securityPath)
//        {
//            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.SecurityPathByIdents<TSecurityContext>>
//                  .SecurityByIdentsExpressionBuilder<TSecurityContext>(this, securityPath);
//        }

//        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
//        {
//            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.SecurityPathByIdents<TSecurityContext>>
//                  .ManySecurityExpressionBuilder<TSecurityContext>(this, securityPath);
//        }

//        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
//        {
//            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.SecurityPathByIdents<TSecurityContext>>
//                  .SingleSecurityExpressionBuilder<TSecurityContext>(this, securityPath);
//        }

//        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.OrSecurityPath securityPath)
//        {
//            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.OrSecurityPath>
//                  .OrBinarySecurityPathExpressionBuilder(this, securityPath);
//        }

//        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.AndSecurityPath securityPath)
//        {
//            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.AndSecurityPath>
//                  .AndBinarySecurityPathExpressionBuilder(this, securityPath);
//        }

//        protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TNestedObject>(SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
//        {
//            return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject>>
//                  .NestedManySecurityExpressionBuilder<TNestedObject>(this, securityPath);
//        }
//    }
//}
