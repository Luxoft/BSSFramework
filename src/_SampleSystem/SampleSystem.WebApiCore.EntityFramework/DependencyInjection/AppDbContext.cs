using Framework.DomainDriven.EntityFramework;

using GenericQueryable;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

using SampleSystem.Domain;

namespace SampleSystem.ServiceEnvironment;

public class AppDbContext(DbContextOptions<AppDbContext> rootOptions, IGenericQueryableExecutor genericQueryableExecutor) : DbContext(rootOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessUnitEmployeeRole>();
        modelBuilder.Entity<Employee>();
        modelBuilder.Entity<BusinessUnit>();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);

        options.ReplaceService<IAsyncQueryProvider, EntityQueryProvider, VisitedEfQueryProvider>();

        options.UseLazyLoadingProxies();

        //options.UseInternalServiceProvider(this.BuildInternalServiceProvider());
    }

    //private IServiceProvider BuildInternalServiceProvider()
    //{
    //    return new ServiceCollection()
    //        .AddEntityFrameworkProxies()
    //        .AddEntityFrameworkSqlServer()
    //        .AddSingleton(genericQueryableExecutor)
    //        .ReplaceScoped<IAsyncQueryProvider, EfEntityQueryProvider>()
    //    .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    //}
}
