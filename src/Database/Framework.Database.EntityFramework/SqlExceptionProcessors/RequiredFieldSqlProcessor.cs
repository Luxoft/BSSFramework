using System.Text.RegularExpressions;

using Framework.Database.DALExceptions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal class RequiredFieldSqlProcessor : ISqlExceptionProcessor
{
    public IReadOnlyCollection<int> ErrorNumbers { get; } = [515];

    public Exception Process(SqlException sqlException, DbUpdateException dbUpdateException, ExceptionProcessingContext context)
    {
        // Cannot insert the value NULL into column 'name', table 'invoicing_sys.dbo.Bank'; column does not allow nulls. UPDATE fails.
        var regex = new Regex(@"Cannot insert the value NULL into column\s+'(.+)',\s+table\s+'(.+)'");

        var matches = regex.Match(sqlException.Message);

        if (!matches.Success)
        {
            return sqlException;
        }

        var columnNameValue = matches.Groups[1].Value;
        var tableValue = matches.Groups[2].Value;

        var entityType = context.GetEntityType(tableValue);

        if (entityType is null)
        {
            return sqlException;
        }

        var entry = dbUpdateException.Entries.FirstOrDefault(e => entityType.ClrType.IsInstanceOfType(e.Entity));

        return new RequiredConstraintDALException(new DomainObjectInfo(entityType.ClrType, ExceptionProcessingContext.GetEntityId(entry)), columnNameValue);
    }
}
