using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.Rules.Builders.QueryablePermissions;

public class SecurityExpressionBuilderFactory<TIdent> : SecurityExpressionBuilderFactoryBase<TIdent>
{
    public SecurityExpressionBuilderFactory(
        IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
        IAuthorizationSystem<TIdent> authorizationSystem,
        ISecurityContextInfoService securityContextInfoService)
    {
        this.HierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.AuthorizationSystem = authorizationSystem;
        this.SecurityContextInfoService = securityContextInfoService;
    }

    public IHierarchicalObjectExpanderFactory<TIdent> HierarchicalObjectExpanderFactory { get; }

    public IAuthorizationSystem<TIdent> AuthorizationSystem { get; }

    public ISecurityContextInfoService SecurityContextInfoService { get; }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.ConditionPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, TIdent, SecurityPath<TDomainObject>.ConditionPath>
                .ConditionBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, TIdent, SecurityPath<TDomainObject>>.ManySecurityExpressionBuilder<TSecurityContext>(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, TIdent, SecurityPath<TDomainObject>>.SingleSecurityExpressionBuilder<TSecurityContext>(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.OrSecurityPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, TIdent, SecurityPath<TDomainObject>.OrSecurityPath>
                .OrBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, TIdent, SecurityPath<TDomainObject>.AndSecurityPath>
                .AndBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TNestedObject>(SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, TIdent, SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject>>
                .NestedManySecurityExpressionBuilder<TNestedObject>(this, securityPath);
    }
}
