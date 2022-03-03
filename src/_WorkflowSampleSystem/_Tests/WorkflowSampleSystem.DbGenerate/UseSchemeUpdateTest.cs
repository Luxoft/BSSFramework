﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Framework.Cap.Abstractions;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHibernate.Tool.hbm2ddl;

using WorkflowSampleSystem.WebApiCore;

namespace WorkflowSampleSystem.DbGenerate
{
    [TestClass]
    public class UseSchemeUpdateTest
    {
        [TestMethod]
        public void LocalDb()
        {
            const string connectionString =
                    "Data Source=.;Initial Catalog=WorkflowSampleSystem;Integrated Security=True;Application Name=WorkflowSampleSystem;TrustServerCertificate=True";
            UseSchemeUpdate(connectionString);
        }

        public static void UseSchemeUpdate(string connectionString)
        {
            RunSchemeUpdate(connectionString);
        }

        private static void RunSchemeUpdate(string connectionString)
        {
            CheckDataBaseAndSchemeExists(connectionString);

            var services = new ServiceCollection();

            services.AddDatabaseSettings(connectionString);

            services.AddSingleton<IDateTimeService>(DateTimeService.Default);
            services.AddSingleton(UserAuthenticationService.CreateFor("neg"));
            services.AddSingleton<ICapTransactionManager, FakeCapTransactionManager>();
            var provider = services.BuildServiceProvider(false);

            var dbSessionFactory = (NHibSessionFactory) provider.GetService<IDBSessionFactory>();
            var cfg = dbSessionFactory?.Configuration;

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
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'authAudit' ) EXEC('CREATE SCHEMA [authAudit]');
IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'workflow' ) EXEC('CREATE SCHEMA [workflow]');";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        private class FakeCapTransactionManager : ICapTransactionManager
        {
            public void Enlist(IDbTransaction dbTransaction)
            {
            }
        }
    }
}
