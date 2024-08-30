using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class AccessorsFilterBuilderFactory<TDomainObject>(
    IEnumerable<IPermissionSystem> permissionSystems,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    ISecurityContextSource securityContextSource) :
    FilterBuilderFactoryBase<TDomainObject, AccessorsFilterBuilder<TDomainObject>>, IAccessorsFilterFactory<TDomainObject>
{
    public AccessorsFilterInfo<TDomainObject> CreateFilter(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath)
    {
        var builder = this.CreateBuilder(securityPath);

        var getAccessorsFunc = LazyHelper.Create(
            () => FuncHelper.Create(
                (TDomainObject domainObject) =>
                {
                    var filter = builder.GetAccessorsFilter(domainObject, securityRule.SafeExpandType);

                    return permissionSystems
                           .SelectMany(ps => ps.GetPermissionSource(securityRule).GetAccessors(filter))
                           .Distinct();
                }));

        return new AccessorsFilterInfo<TDomainObject>(v => getAccessorsFunc.Value(v));
    }


    protected override AccessorsFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.ConditionPath securityPath)
    {
        return new ConditionFilterBuilder<TDomainObject>(securityPath);
    }

    protected override AccessorsFilterBuilder<TDomainObject> CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    {
        return new SingleContextFilterBuilder<TDomainObject, TSecurityContext>(hierarchicalObjectExpanderFactory, securityContextSource, securityPath);
    }

    protected override AccessorsFilterBuilder<TDomainObject> CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    {
        return new ManyContextFilterBuilder<TDomainObject, TSecurityContext>(hierarchicalObjectExpanderFactory, securityContextSource, securityPath);
    }

    protected override AccessorsFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.OrSecurityPath securityPath)
    {
        return new OrFilterBuilder<TDomainObject>(this, securityPath);
    }

    protected override AccessorsFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    {
        return new AndFilterBuilder<TDomainObject>(this, securityPath);
    }

    protected override AccessorsFilterBuilder<TDomainObject> CreateBuilder<TNestedObject>(SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
    {
        var nestedBuilderFactory = new AccessorsFilterBuilderFactory<TNestedObject>(
            permissionSystems,
            hierarchicalObjectExpanderFactory,
            securityContextSource);

        return new NestedManyFilterBuilder<TDomainObject, TNestedObject>(nestedBuilderFactory, securityPath);
    }
}
