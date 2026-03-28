using Framework.Database.DALExceptions;

namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal class ArithmeticOverflowSqlProcessor : ISqlExceptionProcessor
{
    public int ErrorNumber => 8115;

    public Exception Process(HandledGenericAdoException genericAdoException, ExceptionProcessingContext _)
    {
        var sqlException = genericAdoException.SqlException;
        return new ArithmeticOverflowDALException(sqlException.Message, sqlException.Message);
    }
}
