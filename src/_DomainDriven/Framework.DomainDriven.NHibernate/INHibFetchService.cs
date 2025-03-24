namespace Framework.DomainDriven.NHibernate;

public interface INHibFetchService<TSource>
{
    IQueryable<TSource> ApplyFetch(IQueryable<TSource> source, string path);
}
