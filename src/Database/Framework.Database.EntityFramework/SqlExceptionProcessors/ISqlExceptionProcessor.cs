using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal interface ISqlExceptionProcessor
{
    IReadOnlyCollection<int> ErrorNumbers { get; }

    Exception Process(SqlException sqlException, DbUpdateException dbUpdateException, ExceptionProcessingContext context);
}
