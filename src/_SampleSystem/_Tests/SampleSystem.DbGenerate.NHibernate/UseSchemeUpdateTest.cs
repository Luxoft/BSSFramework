﻿using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.NHibernate;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHibernate.Tool.hbm2ddl;

using SampleSystem.DbMigrator;
using SampleSystem.ServiceEnvironment.NHibernate;

namespace SampleSystem.DbGenerate;

[TestClass]
public class UseSchemeUpdateTest
{
    [TestMethod]
    public void LocalDb()
    {
        const string connectionString =
                "Data Source=.;Initial Catalog=SampleSystem;Integrated Security=True;Application Name=SampleSystem;TrustServerCertificate=True";
        UseSchemeUpdate(connectionString);
    }

    public static void UseSchemeUpdate(string connectionString)
    {
        RunSchemeUpdate(connectionString);

        RunFluentMigrator(connectionString);
    }

    private static void RunFluentMigrator(string connectionString)
    {
        var migrator = new BssFluentMigrator(connectionString, typeof(AddLogTable).Assembly);

        migrator.Migrate();
    }

    private static void RunSchemeUpdate(string connectionString)
    {
        CheckDataBaseAndSchemeExists(connectionString);

        var provider = new ServiceCollection()
                       .Self(new SampleSystemNHibernateExtension(false).AddServices)
                       .ReplaceSingleton<IDefaultConnectionStringSource>(new ManualDefaultConnectionStringSource(connectionString))
                       .AddNotImplemented<IDefaultUserAuthenticationService>()
                       .Self(sc => sc.RemoveBy(sd => sd.Lifetime == ServiceLifetime.Scoped))
                       .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        var dbSessionFactory = provider.GetService<NHibSessionEnvironment>();
        var cfg = dbSessionFactory.Configuration;

        var migrate = new SchemaUpdate(cfg);

        migrate.Execute(true, true);

        if (migrate.Exceptions.Any())
        {
            throw new AggregateException(migrate.Exceptions);
        }
    }

    private static void CheckDataBaseAndSchemeExists(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var dataBaseName = connectionStringBuilder.InitialCatalog;

        connectionStringBuilder.InitialCatalog = string.Empty; // If DATABASE don't exists connection failed
        using var connection = new SqlConnection(connectionStringBuilder.ToString());

        connection.Open();
        using (var command = connection.CreateCommand())
        {
            var commandCommandText =
                    $@"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dataBaseName}')   CREATE DATABASE [{dataBaseName}]";
            command.CommandText =
                    commandCommandText;
            command.ExecuteNonQuery();
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandText = $@"USE [{dataBaseName}]
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'appAudit' ) EXEC('CREATE SCHEMA [appAudit]');
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'app' ) EXEC('CREATE SCHEMA [app]');
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'configuration' ) EXEC('CREATE SCHEMA [configuration]');
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'configurationAudit' ) EXEC('CREATE SCHEMA [configurationAudit]');
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'auth' ) EXEC('CREATE SCHEMA [auth]');
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'authAudit' ) EXEC('CREATE SCHEMA [authAudit]');";
            command.ExecuteNonQuery();
        }

        connection.Close();
    }
}
