using Framework.DomainDriven._Visitors;

using GenericQueryable.EntityFramework;

using Microsoft.EntityFrameworkCore;

using SampleSystem.Domain;

namespace SampleSystem.ServiceEnvironment;

public class AppDbContext(DbContextOptions<AppDbContext> rootOptions, IExpressionVisitorContainer expressionVisitorContainer) : DbContext(rootOptions)
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

        options.UseGenericQueryable();
        options.UseLazyLoadingProxies();
        options.AddInterceptors(new GeneralQueryExpressionInterceptor(expressionVisitorContainer));
    }
}
