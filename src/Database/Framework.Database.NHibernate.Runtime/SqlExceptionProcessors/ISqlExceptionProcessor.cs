namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal interface ISqlExceptionProcessor
{
    int ErrorNumber { get; }

    Exception Process(HandledGenericAdoException genericAdoException, ExceptionProcessingContext context);
}
