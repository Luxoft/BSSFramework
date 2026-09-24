using System.Text.RegularExpressions;

using Framework.Database.DALExceptions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal class UniqueIndexSqlProcessor(IDalValidationIdentitySource dalValidationIdentitySource) : ISqlExceptionProcessor
{
    // Violation of UNIQUE KEY constraint 'name'. Cannot insert duplicate key in object 'app.Table'.
    private static readonly Regex ConstraintRegex = new(@"Violation of UNIQUE KEY constraint\s+'(.+)'.\s+Cannot insert duplicate key in object\s+'([a-zA-Z]+\.)?(.+)'");

    // Cannot insert duplicate key row in object 'app.Table' with unique index 'name'. The duplicate key value is (1).
    private static readonly Regex IndexRegex = new(@"Cannot insert duplicate key row in object\s+'([a-zA-Z]+\.)?(.+)'\s+with unique index\s+'(.+)'");

    public IReadOnlyCollection<int> ErrorNumbers { get; } = [2627, 2601];

    public Exception Process(SqlException sqlException, DbUpdateException dbUpdateException, ExceptionProcessingContext context)
    {
        string indexNameValue;
        string tableValue;

        var constraintMatches = ConstraintRegex.Match(sqlException.Message);

        if (constraintMatches.Success)
        {
            indexNameValue = constraintMatches.Groups[1].Value;
            tableValue = constraintMatches.Groups[3].Value;
        }
        else
        {
            var indexMatches = IndexRegex.Match(sqlException.Message);

            if (!indexMatches.Success)
            {
                return sqlException;
            }

            tableValue = indexMatches.Groups[2].Value;
            indexNameValue = indexMatches.Groups[3].Value;
        }

        var entityType = context.GetEntityType(tableValue);

        if (entityType is null)
        {
            return sqlException;
        }

        var uniqueKey = entityType.GetKeys()
                                  .Where(key => !key.IsPrimaryKey())
                                  .FirstOrDefault(key => string.Equals(key.GetName(), indexNameValue, StringComparison.InvariantCultureIgnoreCase));

        var columnNames = uniqueKey?.Properties.Select(property => property.GetColumnName());

        if (columnNames is null)
        {
            var uniqueIndex = entityType.GetIndexes()
                                        .FirstOrDefault(index => index.IsUnique && string.Equals(index.GetDatabaseName(), indexNameValue, StringComparison.InvariantCultureIgnoreCase));

            columnNames = uniqueIndex?.Properties.Select(property => property.GetColumnName());
        }

        if (columnNames is null)
        {
            return sqlException;
        }

        var entry = dbUpdateException.Entries.FirstOrDefault(e => entityType.ClrType.IsInstanceOfType(e.Entity));

        return new UniqueViolationConstraintDALException(
            new UniqueConstraint(
                new DomainObjectInfo(entityType.ClrType, ExceptionProcessingContext.GetEntityId(entry)),
                indexNameValue,
                columnNames,
                dalValidationIdentitySource),
            dalValidationIdentitySource);
    }
}
