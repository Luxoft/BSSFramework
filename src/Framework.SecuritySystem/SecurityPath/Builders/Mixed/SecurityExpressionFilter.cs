namespace Framework.SecuritySystem.Rules.Builders.Mixed;

public class SecurityExpressionFilter<TDomainObject> : ISecurityExpressionFilter<TDomainObject>
{
    private readonly ISecurityExpressionFilter<TDomainObject> hasAccessFilter;

    private readonly ISecurityExpressionFilter<TDomainObject> queryFilter;

    public SecurityExpressionFilter(
            ISecurityExpressionFilter<TDomainObject> hasAccessFilter,
            ISecurityExpressionFilter<TDomainObject> queryFilter)
    {
        this.hasAccessFilter = hasAccessFilter ?? throw new ArgumentNullException(nameof(hasAccessFilter));
        this.queryFilter = queryFilter ?? throw new ArgumentNullException(nameof(queryFilter));
    }

    public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc => this.queryFilter.InjectFunc;

    public Func<TDomainObject, bool> HasAccessFunc => this.hasAccessFilter.HasAccessFunc;

    public IEnumerable<string> GetAccessors(TDomainObject domainObject)
    {
        return this.hasAccessFilter.GetAccessors(domainObject);
    }
}
