using Framework.Core;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven;

public record PropertyPathFetchRule<TSource>(IReadOnlyList<PropertyPath> Paths) : FetchRule<TSource>
{
    public PropertyPathFetchRule<TSource> Compress()
    {
        throw new NotImplementedException();
    }
}
