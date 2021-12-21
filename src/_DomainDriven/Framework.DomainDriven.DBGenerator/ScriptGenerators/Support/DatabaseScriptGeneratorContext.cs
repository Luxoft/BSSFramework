using System;

using Framework.Core.Services;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.Metadata;
using Framework.DomainDriven.NHibernate;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DBGenerator
{
    /// <summary>
    /// Необходимая информация для генератора скриптов по модификации базы данных
    /// </summary>
    public class DatabaseScriptGeneratorContext : IDatabaseScriptGeneratorContext
    {
        public DatabaseScriptGeneratorContext([NotNull]DatabaseName databaseName, [NotNull]ISqlDatabaseFactory sqlDatabaseFactory, [NotNull]AssemblyMetadata assemblyMetadata, [NotNull] IUserAuthenticationService userAuthenticationService)
        {
            if (databaseName == null)
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            if (sqlDatabaseFactory == null)
            {
                throw new ArgumentNullException(nameof(sqlDatabaseFactory));
            }

            if (assemblyMetadata == null)
            {
                throw new ArgumentNullException(nameof(assemblyMetadata));
            }

            if (userAuthenticationService == null)
            {
                throw new ArgumentNullException(nameof(userAuthenticationService));
            }

            this.AssemblyMetadata = assemblyMetadata;
            this.SqlDatabaseFactory = sqlDatabaseFactory;
            this.DatabaseName = databaseName;
            this.UserAuthenticationService = userAuthenticationService;
        }

        /// <summary>
        /// Имя базы данных на которой будет выполнены скрипты
        /// </summary>
        public DatabaseName DatabaseName { get; }

        /// <summary>
        /// Экземпляр Sql сервера на ктором будет выполнены скрипты
        /// </summary>
        public ISqlDatabaseFactory SqlDatabaseFactory { get; }

        /// <summary>
        /// Метаданные доменной модели по которой будут строится скрипты
        /// </summary>
        public AssemblyMetadata AssemblyMetadata { get; }

        public IUserAuthenticationService UserAuthenticationService { get; }
    }
}
