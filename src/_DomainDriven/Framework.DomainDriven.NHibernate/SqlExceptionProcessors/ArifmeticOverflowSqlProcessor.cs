using System;

namespace Framework.DomainDriven.NHibernate.SqlExceptionProcessors;

internal class ArifmeticOverflowSqlProcessor : ISqlExceptionProcessor
{
    public int ErrorNumber => 8115;

    public Exception Process(HandledGenericADOException genericAdoException, ExceptionProcessingContext _)
    {
        var sqlException = genericAdoException.SqlException;
        return new ArifmeticOverflowDALException(sqlException.Message, sqlException.Message);
    }
}
