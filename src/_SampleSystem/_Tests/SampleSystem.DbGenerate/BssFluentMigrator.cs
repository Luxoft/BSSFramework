using System;
using System.Reflection;

using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.DbGenerate
{
    public class BssFluentMigrator
    {
        private readonly IServiceProvider serviceProvider;

        public BssFluentMigrator(string connectionString, params Assembly[] migrationAssemblies) =>
                this.serviceProvider = new ServiceCollection()
                                       .AddFluentMigratorCore()
                                       .ConfigureRunner(
                                                        rb => rb
                                                              .AddSqlServer()
                                                              .WithGlobalConnectionString(connectionString)
                                                              .ScanIn(migrationAssemblies)
                                                              .For.Migrations())
                                       .AddLogging(lb => lb.AddFluentMigratorConsole())
                                       .BuildServiceProvider(false);

        public void Migrate()
        {
            using var scope = this.serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}
