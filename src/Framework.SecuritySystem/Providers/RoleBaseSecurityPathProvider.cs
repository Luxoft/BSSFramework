using Framework.Core;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.Builders._Filter;

namespace Framework.SecuritySystem;

/// <summary>
/// Контекстный провайдер доступа
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public class RoleBaseSecurityPathProvider<TDomainObject> : ISecurityProvider<TDomainObject>
{
    private readonly DomainSecurityRule securityRule;

    private readonly Lazy<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> injectFilterFunc;

    private readonly Lazy<ISecurityExpressionFilter<TDomainObject>> lazyFilter;

    public RoleBaseSecurityPathProvider(
        SecurityPath<TDomainObject> securityPath,
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
    {
        this.securityRule = securityRule;

        var securityExpressionBuilder = securityExpressionBuilderFactory.CreateBuilder(securityPath);

        this.lazyFilter = LazyHelper.Create(() => securityExpressionBuilder.GetFilter(securityRule, securityPath.GetUsedTypes()));
        this.injectFilterFunc = LazyHelper.Create(() => this.lazyFilter.Value.InjectFunc);
    }

    public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable) => this.injectFilterFunc.Value(queryable);

    public bool HasAccess(TDomainObject domainObject) => this.lazyFilter.Value.HasAccessFunc(domainObject);

    public AccessResult GetAccessResult(TDomainObject domainObject)
    {
        if (this.HasAccess(domainObject))
        {
            return AccessResult.AccessGrantedResult.Default;
        }
        else
        {
            return AccessResult.AccessDeniedResult.Create(domainObject, this.securityRule);
        }
    }

    public SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        new SecurityAccessorData.FixedSecurityAccessorData(this.lazyFilter.Value.GetAccessors(domainObject).ToList());
}
