using System.Text.RegularExpressions;

using Framework.Database.DALExceptions;

namespace Framework.Database.NHibernate.SqlExceptionProcessors;

internal abstract class UniqueViolationSqlProcessorBase(IDalValidationIdentitySource dalValidationIdentitySource) : ISqlExceptionProcessor
{
    public abstract int ErrorNumber { get; }

    protected abstract Regex MessageRegex { get; }

    protected abstract (string IndexName, string Table) ExtractMatch(Match match);

    public Exception Process(HandledGenericAdoException genericAdoException, ExceptionProcessingContext context)
    {
        var sqlException = genericAdoException.SqlException;

        var matches = this.MessageRegex.Match(sqlException.Message);

        var indexNameValue = string.Empty;
        var tableValue = string.Empty;

        if (matches.Success)
        {
            (indexNameValue, tableValue) = this.ExtractMatch(matches);
        }

        var possiblePersistentClasses = context.GetPersistentClass(context.CreateTableDescription(string.Empty, string.Empty, tableValue));
        var persistentClass = possiblePersistentClasses.FirstOrDefault();

        if (persistentClass is null)
        {
            return sqlException;
        }

        var table = persistentClass.Table;

        var uniqueKeys = table.UniqueKeyIterator.ToList();

        var uniqueKey = uniqueKeys.FirstOrDefault(z => string.Equals(indexNameValue, z.Name, StringComparison.InvariantCultureIgnoreCase));

        if (uniqueKey is not null)
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
