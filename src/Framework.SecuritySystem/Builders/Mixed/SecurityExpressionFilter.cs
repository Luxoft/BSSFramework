using Framework.SecuritySystem.Builders._Filter;

namespace Framework.SecuritySystem.Builders.Mixed;

public class SecurityExpressionFilter<TDomainObject>(
    ISecurityExpressionFilter<TDomainObject> hasAccessFilter,
    ISecurityExpressionFilter<TDomainObject> queryFilter)
    : ISecurityExpressionFilter<TDomainObject>
{
    public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc => queryFilter.InjectFunc;

    public Func<TDomainObject, bool> HasAccessFunc => hasAccessFilter.HasAccessFunc;

    public IEnumerable<string> GetAccessors(TDomainObject domainObject) => hasAccessFilter.GetAccessors(domainObject);
}
