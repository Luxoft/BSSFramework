using Framework.Core;
using Framework.Core.StringParse;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Exceptions;
using NHibernate.Impl;

namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal class SqlExceptionProcessorInterceptor : IExceptionExpander
{
    private readonly Configuration cfg;

    private readonly IDalValidationIdentitySource dalValidationIdentitySource;

    private readonly Dictionary<int, ISqlExceptionProcessor> processors;

    private readonly ExceptionProcessingContext context;


    internal SqlExceptionProcessorInterceptor(ISessionFactory factory, Configuration cfg, IDalValidationIdentitySource dalValidationIdentitySource)
    {
        this.cfg = cfg;
        this.dalValidationIdentitySource = dalValidationIdentitySource;
        var connectionString = ((SessionFactoryImpl)factory).ConnectionProvider.GetConnection().ConnectionString;

        var parser = new StringParser();
        var defaultDataSourceResult = parser.Add(new StringPattern().WithStart("Data Source=").WithEnd(";"));
        var defaultInitialCatalogResult = parser.Add(new StringPattern().WithStart("Initial Catalog=").WithEnd(";"));
        var parseResult = parser.Evaluate(connectionString);
        var defaultDataSourceName = parseResult.GetResultFor(defaultDataSourceResult);
        var defaultInitialCatalog = parseResult.GetResultFor(defaultInitialCatalogResult);

        this.processors = this.GetProcessors().ToDictionary(z => z.ErrorNumber);

        this.context = new ExceptionProcessingContext(cfg.ClassMappings, new SchemaDescription(defaultInitialCatalog, defaultDataSourceName));
    }

    private IEnumerable<ISqlExceptionProcessor> GetProcessors()
    {
        yield return new RemoveLinkedObjectSqlProcessor();
        yield return new ArithmeticOverflowSqlProcessor();
        yield return new UniqueIndexSqlProcessor(this.dalValidationIdentitySource);
        yield return new RequiredFieldSqlProcessor();
    }


    public Exception? TryExpand(Exception exception) =>
        exception switch
        {
            HandledGenericAdoException adoException => this.InternalProcess(adoException),
            GenericADOException genericAdoException => this.InternalProcess(genericAdoException),
            StaleObjectStateException stateException => this.InternalProcess(stateException),
            _ => null
        };

    private Exception InternalProcess(HandledGenericAdoException exception)
    {
        this.processors.TryGetValue(exception.SqlException.Number, out var exceptionProcessor);

        try
        {
            return (exceptionProcessor ?? DefaultSqlException.Value).Process(exception, this.context);
        }
        catch
        {
            return exception;
        }
    }

    private Exception InternalProcess(GenericADOException exception) =>
        new(
            $"UnHandled ado exception. Please use 'property name=\"sql_exception_converter\">{typeof(SqlExceptionConverter).FullName}, {typeof(SqlExceptionConverter).Assembly.FullName}</property>' in application config file");

    private Exception InternalProcess(StaleObjectStateException exception) => new StaleDomainObjectStateException(this.cfg.GetClassMapping(exception.EntityName).MappedClass, exception.Identifier, exception);
}
