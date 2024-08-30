using Framework.Core;
using Framework.SecuritySystem.Builders._Factory;

namespace Framework.SecuritySystem;

/// <summary>
/// Контекстный провайдер доступа
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public class RoleBaseSecurityPathProvider<TDomainObject>(
    ISecurityFilterFactory<TDomainObject> securityFilterFactory,
    IAccessorsFilterFactory<TDomainObject> accessorsFilterFactory,
    DomainSecurityRule.RoleBaseSecurityRule securityRule,
    SecurityPath<TDomainObject> securityPath)
    : ISecurityProvider<TDomainObject>
{
    private readonly Lazy<SecurityFilterInfo<TDomainObject>> lazySecurityFilter =
        LazyHelper.Create(() => securityFilterFactory.CreateFilter(securityRule, securityPath));

    private readonly Lazy<AccessorsFilterInfo<TDomainObject>> lazyAccessorsFilter =
        LazyHelper.Create(() => accessorsFilterFactory.CreateFilter(securityRule, securityPath));

    public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable) =>
        this.lazySecurityFilter.Value.InjectFunc(queryable);

    public bool HasAccess(TDomainObject domainObject) => this.lazySecurityFilter.Value.HasAccessFunc(domainObject);

    public AccessResult GetAccessResult(TDomainObject domainObject)
    {
        if (this.HasAccess(domainObject))
        {
            return AccessResult.AccessGrantedResult.Default;
        }
        else
        {
            return AccessResult.AccessDeniedResult.Create(domainObject, securityRule);
        }
    }

    public SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        new SecurityAccessorData.FixedSecurityAccessorData(this.lazyAccessorsFilter.Value.GetAccessorsFunc(domainObject).ToList());
}
