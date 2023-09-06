using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders.QueryablePermissions;

public class SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : SecurityExpressionBuilderFactoryBase<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public SecurityExpressionBuilderFactory(
        IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
        IAuthorizationSystem<TIdent> authorizationSystem,
        ISecurityContextInfoService<TIdent> securityContextInfoService)
    {
        this.HierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.AuthorizationSystem = authorizationSystem;
        this.SecurityContextInfoService = securityContextInfoService;
    }

    public IHierarchicalObjectExpanderFactory<TIdent> HierarchicalObjectExpanderFactory { get; }

    public IAuthorizationSystem<TIdent> AuthorizationSystem { get; }

    public ISecurityContextInfoService<TIdent> SecurityContextInfoService { get; }

    protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.ConditionPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.ConditionPath>
                .ConditionBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    {
        return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>>.ManySecurityExpressionBuilder<TSecurityContext>(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    {
        return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>>.SingleSecurityExpressionBuilder<TSecurityContext>(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.OrSecurityPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.OrSecurityPath>
                .OrBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.AndSecurityPath>
                .AndBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TNestedObject>(SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
    {
        return new SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject>>
                .NestedManySecurityExpressionBuilder<TNestedObject>(this, securityPath);
    }
}
