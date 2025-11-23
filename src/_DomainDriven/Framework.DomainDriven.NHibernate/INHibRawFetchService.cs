namespace Framework.DomainDriven.NHibernate;

public interface INHibRawFetchService<TSource>
{
    IQueryable<TSource> ApplyFetch(IQueryable<TSource> source, string path);
}
