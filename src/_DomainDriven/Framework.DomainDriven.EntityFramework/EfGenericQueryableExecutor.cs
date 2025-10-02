using GenericQueryable;

using Microsoft.EntityFrameworkCore;

namespace Framework.DomainDriven.EntityFramework;

public class EfGenericQueryableExecutor : GenericQueryableExecutor
{
    protected override Type ExtensionsType { get; } = typeof(EntityFrameworkQueryableExtensions);

    protected override IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, string path) => source.Include(path);
}
