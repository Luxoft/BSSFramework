using System.Text.RegularExpressions;

using Framework.Database.DALExceptions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal class RemoveLinkedObjectSqlProcessor : ISqlExceptionProcessor
{
    public IReadOnlyCollection<int> ErrorNumbers { get; } = [547];

    public Exception Process(SqlException sqlException, DbUpdateException dbUpdateException, ExceptionProcessingContext context)
    {
        // The DELETE statement conflicted with the REFERENCE constraint "FK_SqlParserTestObjContainer_includedObjectId_SqlParserTestObj". The conflict occurred in database "SampleSystem", table "app.SqlParserTestObjContainer", column 'includedObjectId'.
        try
        {
            var regex = new Regex(@"The conflict occurred in database\s+""(.+)"",\s+table\s+""([a-zA-Z]+\.)?(.+)"",\s+column\s+'(.+)'");

            var matches = regex.Match(sqlException.Message);

            if (!matches.Success)
            {
                return sqlException;
            }

            var tableValue = matches.Groups[3].Value;
            var columnValue = matches.Groups[4].Value;

            var sourceEntityType = context.GetEntityType(tableValue);

            if (sourceEntityType is null)
            {
                return sqlException;
            }

            var foreignKey = sourceEntityType.GetForeignKeys()
                                             .FirstOrDefault(fk => fk.Properties.Any(property => string.Equals(property.GetColumnName(), columnValue, StringComparison.InvariantCultureIgnoreCase)));

            if (foreignKey is null)
            {
                return sqlException;
            }

            var targetType = foreignKey.PrincipalEntityType.ClrType;
            var propertyName = foreignKey.DependentToPrincipal?.Name ?? foreignKey.Properties[0].Name;

            return new RemoveLinkedObjectsDALException(new LinkedObjects(sourceEntityType.ClrType, targetType, propertyName), string.Empty);
        }
        catch (Exception e)
        {
            return new DALException("Object cannot be deleted", new AggregateException(sqlException, e));
        }
    }
}
