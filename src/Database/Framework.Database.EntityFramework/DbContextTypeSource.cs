using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework;

public class DbContextTypeSource<TPrimaryDbContextType>(
    IServiceProvider serviceProvider,
    DbContextTypeSourceState state,
    [FromKeyedServices(IDbContextTypeSource.SecondaryKey)]
    IEnumerable<Type> secondaryTypes) : IDbContextTypeSource
{
    public Type GetDbContextType(Type domainObjectType) =>
        state.Dictionary.GetOrAdd(
            domainObjectType,
            _ =>
            {
                var request = from dbContextType in new[] { typeof(TPrimaryDbContextType) }.Concat(secondaryTypes)

                              let dbContext = (DbContext)serviceProvider.GetRequiredService(dbContextType)

                              where dbContext.Model.FindEntityType(domainObjectType) != null

                              select dbContextType;

                return request.SingleOrDefault()

                       ?? throw new InvalidOperationException($"No DbContext type for {domainObjectType} not found.");
            });

    public Type PrimaryDbContextType { get; } = typeof(TPrimaryDbContextType);
}
