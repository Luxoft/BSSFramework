namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal class DefaultSqlException : ISqlExceptionProcessor
{
    private static readonly Lazy<DefaultSqlException> InstanceLazy = new Lazy<DefaultSqlException>(()=> new DefaultSqlException(), true);
    public static DefaultSqlException Value
    {
        get { return InstanceLazy.Value; }
    }
    private DefaultSqlException()
    {

    }
    public int ErrorNumber
    {
        get { throw new NotImplementedException(); }
    }

    public Exception Process(HandledGenericAdoException genericAdoException, ExceptionProcessingContext context)
    {
        return genericAdoException.SqlException;
    }
}
