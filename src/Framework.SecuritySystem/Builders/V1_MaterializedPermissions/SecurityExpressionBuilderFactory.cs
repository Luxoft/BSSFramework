using Framework.HierarchicalExpand;
using Framework.SecuritySystem.Builders._Base;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.PermissionOptimization;

namespace Framework.SecuritySystem.Builders.V1_MaterializedPermissions;

public class SecurityExpressionBuilderFactory(
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    IEnumerable<IPermissionSystem> permissionSystems,
    ISecurityContextSource securityContextSource,
    IRuntimePermissionOptimizationService permissionOptimizationService)
    : SecurityExpressionBuilderFactoryBase
{
    public IHierarchicalObjectExpanderFactory<Guid> HierarchicalObjectExpanderFactory { get; } = hierarchicalObjectExpanderFactory;

    public IEnumerable<IPermissionSystem> PermissionSystems { get; } = permissionSystems;

    public ISecurityContextSource SecurityContextInfoService { get; } = securityContextSource;

    public IRuntimePermissionOptimizationService PermissionOptimizationService { get; } = permissionOptimizationService;

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.ConditionPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, SecurityPath<TDomainObject>.ConditionPath>
                .ConditionBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, SecurityPath<TDomainObject>>.ManySecurityExpressionBuilder<TSecurityContext>(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, SecurityPath<TDomainObject>>.SingleSecurityExpressionBuilder<TSecurityContext>(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.OrSecurityPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, SecurityPath<TDomainObject>.OrSecurityPath>
                .OrBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, SecurityPath<TDomainObject>.AndSecurityPath>
                .AndBinarySecurityPathExpressionBuilder(this, securityPath);
    }

    protected override ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TNestedObject>(SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
    {
        return new SecurityExpressionBuilderBase<TDomainObject, SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject>>
                .NestedManySecurityExpressionBuilder<TNestedObject>(this, securityPath);
    }
}
