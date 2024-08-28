namespace Framework.SecuritySystem.Builders._Filter;


public interface ISecurityExpressionFilter<TDomainObject>
{
    Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; }

    Func<TDomainObject, bool> HasAccessFunc { get; }

    IEnumerable<string> GetAccessors(TDomainObject domainObject);
}
