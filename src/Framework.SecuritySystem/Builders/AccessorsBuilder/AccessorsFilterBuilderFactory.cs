using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class AccessorsFilterBuilderFactory<TDomainObject>(IServiceProvider serviceProvider, IEnumerable<IPermissionSystem> permissionSystems) :
    IAccessorsFilterFactory<TDomainObject>
{
    public AccessorsFilterInfo<TDomainObject> CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        var accessorsFilterInfoList = permissionSystems.Select(permissionSystem =>
        {
            var factoryType = typeof(AccessorsFilterBuilderFactory<,>).MakeGenericType(permissionSystem.PermissionType, typeof(TDomainObject));

            var factory = (IAccessorsFilterFactory<TDomainObject>)ActivatorUtilities.CreateInstance(serviceProvider, factoryType, permissionSystem);

            return factory.CreateFilter(securityRule, securityPath);
        }).ToList();

        return new AccessorsFilterInfo<TDomainObject>(
            domainObject => accessorsFilterInfoList.SelectMany(accessorsFilterInfo => accessorsFilterInfo.GetAccessorsFunc(domainObject))
                                                   .Distinct(StringComparer.CurrentCultureIgnoreCase));
    }
}

public class AccessorsFilterBuilderFactory<TPermission, TDomainObject>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory) :
    FilterBuilderFactoryBase<TDomainObject, AccessorsFilterBuilder<TPermission, TDomainObject>>,
    IAccessorsFilterFactory<TDomainObject>
{
    public AccessorsFilterInfo<TDomainObject> CreateFilter(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath)
    {
        var restrictionFilterInfoList = securityRule.GetSafeSecurityContextRestrictionFilters().ToList();

        var builder = this.CreateBuilder(securityPath, restrictionFilterInfoList);

        var getAccessorsFunc = LazyHelper.Create(
            () => FuncHelper.Create(
                (TDomainObject domainObject) =>
                {
                    var filter = builder.GetAccessorsFilter(domainObject, securityRule.GetSafeExpandType());

                    return permissionSystem.GetPermissionSource(securityRule).GetAccessors(filter);
                }));

        return new AccessorsFilterInfo<TDomainObject>(v => getAccessorsFunc.Value(v));
    }

    protected override AccessorsFilterBuilder<TPermission, TDomainObject> CreateBuilder(
        SecurityPath<TDomainObject>.ConditionPath securityPath)
    {
        return new ConditionFilterBuilder<TPermission, TDomainObject>(securityPath);
    }

    protected override AccessorsFilterBuilder<TPermission, TDomainObject> CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
        SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
    {
        return new SingleContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
            permissionSystem,
            hierarchicalObjectExpanderFactory,
            securityPath,
            restrictionFilterInfo);
    }

    protected override AccessorsFilterBuilder<TPermission, TDomainObject> CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath,
        SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
    {
        return new ManyContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
            permissionSystem,
            hierarchicalObjectExpanderFactory,
            securityPath,
            restrictionFilterInfo);
    }

    protected override AccessorsFilterBuilder<TPermission, TDomainObject> CreateBuilder(
        SecurityPath<TDomainObject>.OrSecurityPath securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
    {
        return new OrFilterBuilder<TPermission, TDomainObject>(this, securityPath, restrictionFilterInfoList);
    }

    protected override AccessorsFilterBuilder<TPermission, TDomainObject> CreateBuilder(
        SecurityPath<TDomainObject>.AndSecurityPath securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
    {
        return new AndFilterBuilder<TPermission, TDomainObject>(this, securityPath, restrictionFilterInfoList);
    }

    protected override AccessorsFilterBuilder<TPermission, TDomainObject> CreateBuilder<TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
    {
        var nestedBuilderFactory = new AccessorsFilterBuilderFactory<TPermission, TNestedObject>(
            permissionSystem,
            hierarchicalObjectExpanderFactory);

        return new NestedManyFilterBuilder<TPermission, TDomainObject, TNestedObject>(
            nestedBuilderFactory,
            securityPath,
            restrictionFilterInfoList);
    }
}
