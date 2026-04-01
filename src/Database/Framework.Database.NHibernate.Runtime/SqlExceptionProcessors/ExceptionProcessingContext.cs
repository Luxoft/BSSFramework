using System.Text;

using NHibernate.Mapping;

namespace Framework.Database.NHibernate.SqlExceptionProcessors;

public readonly struct TableDescription : IEquatable<TableDescription>
{
    public string Schema { get; }
    public string Catalog { get; }
    public string Name { get; }


    internal TableDescription(string catalog, string schema, string name)
            : this()
    {
        this.Schema = schema;
        this.Name = name;
        this.Catalog = catalog;
    }

    public bool Equals(TableDescription other) => string.Equals(this.ToString(), other.ToString(), StringComparison.InvariantCultureIgnoreCase);

    public override string ToString()
    {
        var builder = new StringBuilder();
        if (this.Catalog != null)
        {
            builder.Append(this.Catalog + ".");
        }
        if (this.Schema != null)
        {
            builder.Append(this.Schema + ".");
        }
        builder.Append(this.Name).Append(')');
        return builder.ToString();
    }
    public override bool Equals(object obj) => string.Equals(this.ToString(), obj.ToString());

    public override int GetHashCode() => this.ToString().GetHashCode();
}

public class ExceptionProcessingContext
{
    private readonly ICollection<PersistentClass> nhibernatePersistentClass;
    private readonly SchemaDescription defaultSchemaDescription;
    private readonly Dictionary<TableDescription, IReadOnlyList<PersistentClass>> tableNameToPersistentClass;
    private readonly IReadOnlyList<string> removingSymbols = [".[dbo]", "[", "]"];

    public ExceptionProcessingContext(ICollection<PersistentClass> nhibernatePersistentClass,
                                      SchemaDescription defaultSchemaDescription)
    {
        this.nhibernatePersistentClass = nhibernatePersistentClass;
        this.defaultSchemaDescription = defaultSchemaDescription;
        Func<Table, TableDescription> createTableDescriptionFunc = z => this.CreateTableDescription(z);

        this.tableNameToPersistentClass =
            nhibernatePersistentClass
                .GroupBy(z => createTableDescriptionFunc(z.Table))
                .ToDictionary(z => z.Key, z => (IReadOnlyList<PersistentClass>)z.ToList());
    }

    public TableDescription CreateTableDescription(Table table) => this.CreateTableDescription(table.Catalog, table.Schema, table.Name);

    public TableDescription CreateTableDescription(string initialCatalog, string schema, string tableName) =>
        new(
            string.IsNullOrWhiteSpace(initialCatalog) ? this.defaultSchemaDescription.InitialCatalog : this.TryRemoveSymbols(initialCatalog),
            string.IsNullOrWhiteSpace(schema) ? string.Empty : this.TryRemoveSymbols(schema.ToLower()),
            this.TryRemoveSymbols(tableName.ToLower()));

    public ICollection<PersistentClass> NhibernatePersistentClass => this.nhibernatePersistentClass;

    public IEnumerable<PersistentClass> GetPersistentClass(TableDescription tableDescription)
    {
        IReadOnlyList<PersistentClass> result;
        if(!this.tableNameToPersistentClass.TryGetValue(tableDescription, out result))
        {
            //костыль
            var results = this.tableNameToPersistentClass.Where(z => string.Equals(z.Key.Name, tableDescription.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (results.Count == 1)
            {
                return results.First().Value;
            }

            throw new ArgumentNullException($"{tableDescription.ToString()} do not register in exception monitoring system");
        }

        return result;
    }

    private string TryRemoveSymbols(string value) =>
        this.removingSymbols.Aggregate(value, (prev, symbol) =>
        {
            var index = prev.IndexOf(symbol);
            var length = symbol.Length;
            if (-1 != index)
            {
                return prev.Remove(index, length);
            }
            return prev;
        });

    public struct TableDescription : IEquatable<TableDescription>
    {
        public string Schema { get; }
        public string Catalog { get; }
        public string Name { get; }


        internal TableDescription(string catalog, string schema, string name) : this()
        {
            this.Schema = schema;
            this.Name = name;
            this.Catalog = catalog;

        }

        public bool Equals(TableDescription other)
        {
            var getPropertyFuncs =
                    new Func<TableDescription, string>[]
                    {
                            z => z.Name,
                            z => z.Schema,
                            z => z.Catalog
                    };

            var localThis = this;

            return getPropertyFuncs.All(
                                        propertyFunc =>
                                                string.Equals(propertyFunc(localThis), propertyFunc(other), StringComparison.InvariantCultureIgnoreCase));
        }

        public override string ToString() => string.Join(".", new[]{this.Catalog, this.Schema, this.Name}.Where(z=>null != z));

        public override bool Equals(object obj) => string.Equals(this.ToString(), obj.ToString());

        public override int GetHashCode() => this.ToString().GetHashCode();
    }
}
