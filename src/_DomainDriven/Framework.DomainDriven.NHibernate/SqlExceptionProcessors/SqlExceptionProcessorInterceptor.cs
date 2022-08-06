using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core.StringParse;
using Framework.Exceptions;

using JetBrains.Annotations;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Exceptions;
using NHibernate.Impl;

namespace Framework.DomainDriven.NHibernate.SqlExceptionProcessors
{
    internal class SqlExceptionProcessorInterceptor : IExceptionProcessor
    {
        private readonly Configuration _cfg;

        private readonly Dictionary<int, ISqlExceptionProcessor> _processors;
        private readonly ExceptionProcessingContext _context;


        internal SqlExceptionProcessorInterceptor([NotNull] ISessionFactory factory, [NotNull] Configuration cfg)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));


            this._cfg = cfg;
            var connectionString = ((SessionFactoryImpl)factory).ConnectionProvider.GetConnection().ConnectionString;

            var parser = new StringParser();
            var defaultDataSourceResult = parser.Add(new StringPattern().WithStart("Data Source=").WithEnd(";"));
            var defaultInitialCatalogResult = parser.Add(new StringPattern().WithStart("Initial Catalog=").WithEnd(";"));
            var parseResult = parser.Evaluate(connectionString);
            var defaultDataSourceName = parseResult.GetResultFor(defaultDataSourceResult);
            var defaultInitialCatalog = parseResult.GetResultFor(defaultInitialCatalogResult);

            this._processors = this.GetProcessors().ToDictionary(z => z.ErrorNumber);

            this._context = new ExceptionProcessingContext(cfg.ClassMappings, new SchemaDescription(defaultInitialCatalog, defaultDataSourceName));
        }


        //internal SqlExceptionProcessorInterceptor(ICollection<PersistentClass> classMappings, SchemaDescription defaultSettings)
        //{
        //    _processors = GetProcessors().ToDictionary(z => z.ErrorNumber);

        //    _context = new ExceptionProcessingContext(classMappings, defaultSettings);
        //}


        private IEnumerable<ISqlExceptionProcessor> GetProcessors()
        {
            yield return new RemoveLinkedObjectSqlProcessor();
            yield return new ArifmeticOverflowSqlProcessor();
            yield return new UniqueIndexSqlProcessor();
            yield return new RequiredFieldSqlProcessor();
        }


        public Exception Process([NotNull] Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            if (exception is HandledGenericADOException)
            {
                return this.InternalProcess(exception as HandledGenericADOException);
            }
            else if (exception is GenericADOException)
            {
                return this.InternalProcess(exception as GenericADOException);
            }
            else if (exception is StaleObjectStateException)
            {
                return this.InternalProcess(exception as StaleObjectStateException);
            }
            else
            {
                return exception;
            }
        }


        private Exception InternalProcess(HandledGenericADOException exception)
        {
            ISqlExceptionProcessor exceptionProcessor;

           this._processors.TryGetValue(exception.SqlException.Number, out exceptionProcessor);

            try
            {
                return (exceptionProcessor ?? DefaultSqlException.Value).Process(exception, this._context);
            }
            catch
            {
                return exception;
            }
        }

        private Exception InternalProcess(GenericADOException exception)
        {
            return new Exception(
                $"UnHandled ado exception. Please use 'property name=\"sql_exception_converter\">{typeof(SQLExceptionConverter).FullName}, {typeof(SQLExceptionConverter).Assembly.FullName}</property>' in application config file");
        }

        private Exception InternalProcess(StaleObjectStateException exception)
        {
            return new StaleDomainObjectStateException(exception, this._cfg.GetClassMapping(exception.EntityName).MappedClass, exception.Identifier);
        }
    }
}
