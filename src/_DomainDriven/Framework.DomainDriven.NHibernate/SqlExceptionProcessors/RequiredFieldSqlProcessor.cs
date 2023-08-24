using System.Text.RegularExpressions;

using Framework.DomainDriven.DALExceptions;

using NHibernate.Mapping;

namespace Framework.DomainDriven.NHibernate.SqlExceptionProcessors;

internal class RequiredFieldSqlProcessor : ISqlExceptionProcessor
{
    public int ErrorNumber => 515;

    public Exception Process(HandledGenericADOException genericAdoException, ExceptionProcessingContext context)
    {
        // Cannot insert the value NULL into column 'name', table 'invoicing_sys.dbo.Bank'; column does not allow nulls. UPDATE fails.
        var regex = new Regex(@"Cannot insert the value NULL into column\s+'(.+)',\s+table\s+'(.+)'");

        var message = genericAdoException.SqlException.Message;

        var matches = regex.Match(message);

        var columnNameValue = string.Empty;
        var tableValue = string.Empty;

        if (matches.Success)
        {
            columnNameValue = matches.Groups[1].Value;
            tableValue = matches.Groups[2].Value;
        }

        var persistentClass = context.NhibernatePersistentClass.FirstOrDefault(z => CompareEntity(z, tableValue));
        if (persistentClass != null)
        {
            return new RequiredConstraintDALException(new DomainObjectInfo(persistentClass.MappedClass, genericAdoException.EntityId), columnNameValue);
        }

        return genericAdoException.SqlException;
    }

    private static bool CompareEntity(PersistentClass z, string tableValue)
    {
        var classParts = z.EntityName.Split('.');
        var tableParts = tableValue.Split('.');

        return classParts.Last().Equals(tableParts.Last(), StringComparison.InvariantCultureIgnoreCase);
    }
}
