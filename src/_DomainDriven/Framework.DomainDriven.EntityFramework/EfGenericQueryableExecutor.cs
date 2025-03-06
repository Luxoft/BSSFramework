using Framework.GenericQueryable;

using Microsoft.EntityFrameworkCore;

namespace Framework.DomainDriven.EntityFramework;

public class EfGenericQueryableExecutor : GenericQueryableExecutor
{
    protected override Type ExtensionsType { get; } = typeof(EntityFrameworkQueryableExtensions);
}
