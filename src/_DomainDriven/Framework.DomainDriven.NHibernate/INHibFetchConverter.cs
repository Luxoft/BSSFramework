using GenericQueryable.Fetching;

namespace Framework.DomainDriven.NHibernate;

public interface INHibFetchConverter
{
    PropertyFetchRule<TSource> Convert<TSource>(FetchRule<TSource> fetchRule);
}
