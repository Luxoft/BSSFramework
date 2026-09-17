using Framework.Database.DALExceptions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal class ArithmeticOverflowSqlProcessor : ISqlExceptionProcessor
{
    public IReadOnlyCollection<int> ErrorNumbers { get; } = [8115];

    public Exception Process(SqlException sqlException, DbUpdateException dbUpdateException, ExceptionProcessingContext context) =>
        new ArithmeticOverflowDALException(sqlException.Message, sqlException.Message);
}
