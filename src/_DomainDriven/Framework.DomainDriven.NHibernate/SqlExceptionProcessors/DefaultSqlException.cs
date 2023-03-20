using System;

namespace Framework.DomainDriven.NHibernate.SqlExceptionProcessors;

internal class DefaultSqlException : ISqlExceptionProcessor
{
    private static readonly Lazy<DefaultSqlException> _instanceLazy = new Lazy<DefaultSqlException>(()=> new DefaultSqlException(), true);
    public static DefaultSqlException Value
    {
        get { return _instanceLazy.Value; }
    }
    private DefaultSqlException()
    {

    }
    public int ErrorNumber
    {
        get { throw new NotImplementedException(); }
    }

    public Exception Process(HandledGenericADOException genericADOException, ExceptionProcessingContext context)
    {
        return genericADOException.SqlException;
    }
}
