using System.Text.RegularExpressions;

using Framework.Application.DALExceptions;
using Framework.Database.DALExceptions;

namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal class UniqueIndexSqlProcessor(IDalValidationIdentitySource dalValidationIdentitySource) : ISqlExceptionProcessor
{
    public int ErrorNumber => 2627;

    public Exception Process(HandledGenericAdoException genericAdoException, ExceptionProcessingContext context)
    {
        var sqlException = genericAdoException.SqlException;

        var message = sqlException.Message;

        var regex = new Regex(@"Violation of UNIQUE KEY constraint\s+'(.+)'.\s+Cannot insert duplicate key in object\s+'([a-zA-Z]+\.)?(.+)'");

        var matches = regex.Match(message);

        var indexNameValue = string.Empty;
        var tableValue = string.Empty;

        if (matches.Success)
        {
            indexNameValue = matches.Groups[1].Value;
            tableValue = matches.Groups[3].Value;
        }

        var possiblePersistentClasses = context.GetPersistentClass(context.CreateTableDescription(string.Empty, string.Empty, tableValue));
        var persistentClass = possiblePersistentClasses.FirstOrDefault();

        if (persistentClass == null)
        {
            return sqlException;
        }

        var table = persistentClass.Table;

        var uniqueKeys = table.UniqueKeyIterator.ToList();

        var uniqueKey = uniqueKeys.FirstOrDefault(z => string.Equals(indexNameValue, z.Name, StringComparison.InvariantCultureIgnoreCase));

        if (uniqueKey != null)
        {
            return new UniqueViolationConstraintDALException(
                new UniqueConstraint(
                    new DomainObjectInfo(
                        persistentClass.MappedClass,
                        genericAdoException.EntityId),
                    uniqueKey.Name,
                    uniqueKey.ColumnIterator.Select(z => z.Name),
                    dalValidationIdentitySource),
                dalValidationIdentitySource);
        }

        return sqlException;
    }
}
