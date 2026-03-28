namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal class DefaultSqlException : ISqlExceptionProcessor
{
    private static readonly Lazy<DefaultSqlException> InstanceLazy = new Lazy<DefaultSqlException>(()=> new DefaultSqlException(), true);
    public static DefaultSqlException Value => InstanceLazy.Value;

    private DefaultSqlException()
    {

    }
    public int ErrorNumber => throw new NotImplementedException();

    public Exception Process(HandledGenericAdoException genericAdoException, ExceptionProcessingContext context) => genericAdoException.SqlException;
}
