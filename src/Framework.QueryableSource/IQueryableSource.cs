namespace Framework.QueryableSource;

public interface IQueryableSource
{
    IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class;
}
