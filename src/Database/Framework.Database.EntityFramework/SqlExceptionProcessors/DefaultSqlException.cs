using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal class DefaultSqlException : ISqlExceptionProcessor
{
    private static readonly Lazy<DefaultSqlException> InstanceLazy = new(() => new DefaultSqlException(), true);

    public static DefaultSqlException Value => InstanceLazy.Value;

    private DefaultSqlException()
    {
    }

    public IReadOnlyCollection<int> ErrorNumbers => throw new NotImplementedException();

    public Exception Process(SqlException sqlException, DbUpdateException dbUpdateException, ExceptionProcessingContext context) => sqlException;
}
