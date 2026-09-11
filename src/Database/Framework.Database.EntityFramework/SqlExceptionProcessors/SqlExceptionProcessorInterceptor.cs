using Framework.Core;
using Framework.Database.DALExceptions;
using Framework.Database;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal class SqlExceptionProcessorInterceptor : IEfSqlExceptionExpander
{
    private readonly IDalValidationIdentitySource dalValidationIdentitySource;

    private readonly Dictionary<int, ISqlExceptionProcessor> processors;

    public SqlExceptionProcessorInterceptor(IDalValidationIdentitySource dalValidationIdentitySource)
    {
        this.dalValidationIdentitySource = dalValidationIdentitySource;
        this.processors = this.GetProcessors()
                               .SelectMany(processor => processor.ErrorNumbers.Select(errorNumber => (errorNumber, processor)))
                               .ToDictionary(pair => pair.errorNumber, pair => pair.processor);
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
            DbUpdateConcurrencyException concurrencyException => InternalProcess(concurrencyException),
            DbUpdateException { InnerException: SqlException sqlException } dbUpdateException => this.InternalProcess(dbUpdateException, sqlException),
            _ => null
        };

    private Exception InternalProcess(DbUpdateException dbUpdateException, SqlException sqlException)
    {
        var model = dbUpdateException.Entries.FirstOrDefault()?.Context.Model;

        if (model is null)
        {
            return dbUpdateException;
        }

        this.processors.TryGetValue(sqlException.Number, out var exceptionProcessor);

        var context = new ExceptionProcessingContext(model);

        try
        {
            return (exceptionProcessor ?? DefaultSqlException.Value).Process(sqlException, dbUpdateException, context);
        }
        catch
        {
            return dbUpdateException;
        }
    }

    private static Exception InternalProcess(DbUpdateConcurrencyException concurrencyException)
    {
        var entry = concurrencyException.Entries.FirstOrDefault();

        return new StaleDomainObjectStateException(
            entry?.Entity.GetType() ?? typeof(object),
            ExceptionProcessingContext.GetEntityId(entry),
            concurrencyException);
    }
}
