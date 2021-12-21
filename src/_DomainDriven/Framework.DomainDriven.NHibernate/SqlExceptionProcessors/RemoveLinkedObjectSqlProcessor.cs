using System;
using System.Linq;
using System.Text.RegularExpressions;

using Framework.Core;
using Framework.Exceptions;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.NHibernate.SqlExceptionProcessors
{
    internal class RemoveLinkedObjectSqlProcessor : ISqlExceptionProcessor
    {
        public int ErrorNumber => 547;

        public Exception Process(HandledGenericADOException genericAdoException, ExceptionProcessingContext context)
        {
            // The DELETE statement conflicted with the REFERENCE constraint "FK_SqlParserTestObjContainer_includedObjectId_SqlParserTestObj". The conflict occurred in database "SampleSystem", table "app.SqlParserTestObjContainer", column 'includedObjectId'.
            var sqlException = genericAdoException.SqlException;
            try
            {
                var regex = new Regex(@"The conflict occurred in database\s+""(.+)"",\s+table\s+""([a-zA-Z]+\.)?(.+)"",\s+column\s+'(.+)'");

                var matches = regex.Match(sqlException.Message);

                var database = string.Empty;
                var table = string.Empty;
                var column = string.Empty;

                if (matches.Success)
                {
                    database = matches.Groups[1].Value;
                    table = matches.Groups[3].Value;
                    column = matches.Groups[4].Value;
                }

                var tableName = table;
                var columnNameWithBrackets = column;

                var persistentClasses = context.GetPersistentClass(context.CreateTableDescription(string.Empty, database, tableName));

                var pairs = persistentClasses.SelectMany(z => z.Table.ForeignKeyIterator.Select(q => new { FromPersistentClass = z, ForeighKey = q })).ToList();

                if (!pairs.Any())
                {
                    return sqlException;
                }

                var linkedTables = pairs
                                   .Where(z => string.Equals(columnNameWithBrackets, z.ForeighKey.Columns.FirstOrDefault()?.Name, StringComparison.CurrentCultureIgnoreCase))
                                   .Select(z => new { PersistentClass = z.FromPersistentClass, z.ForeighKey.ReferencedTable })
                                   .ToList();

                var linkedTable = linkedTables.OrderBy(z => z.PersistentClass.MappedClass.GetCustomAttributes<TableAttribute>().Any() ? 1 : 0).FirstOrDefault();


                var referencePersistentClass = context.GetPersistentClass(context.CreateTableDescription(linkedTable.ReferencedTable))
                                                        .OrderBy(z => z.MappedClass.GetCustomAttributes<TableAttribute>().Any() ? 1 : 0)
                                                        .FirstOrDefault();

                var linkedProperty = linkedTable.PersistentClass.PropertyIterator.Single(z => z.ColumnIterator.Any(q => string.Equals(q.Text, columnNameWithBrackets, StringComparison.InvariantCultureIgnoreCase))).Name;

                return new RemoveLinkedObjectsDALException(
                                                           new LinkedObjects(
                                                                             linkedTable.PersistentClass.MappedClass,
                                                                             referencePersistentClass.MappedClass,
                                                                             linkedProperty),
                                                           string.Empty);
            }
            catch (Exception e)
            {
                throw new BusinessLogicException(new AggregateException(sqlException, e), "Object cannot be deleted");
            }
        }
    }
}
