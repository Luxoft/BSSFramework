using System.Text;

using NHibernate.Mapping;

namespace Framework.DomainDriven.NHibernate.SqlExceptionProcessors;

public struct TableDescription : IEquatable<TableDescription>
{
    public string Schema { get; private set; }
    public string Catalog { get; private set; }
    public string Name { get; private set; }


    internal TableDescription(string catalog, string schema, string name)
            : this()
    {
        this.Schema = schema;
        this.Name = name;
        this.Catalog = catalog;
    }

    public bool Equals(TableDescription other)
    {
        return string.Equals(this.ToString(), other.ToString(), StringComparison.InvariantCultureIgnoreCase);
    }

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
    public override bool Equals(object obj)
    {
        return string.Equals(this.ToString(), obj.ToString());
    }
    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }
}

public class ExceptionProcessingContext
{
    private readonly ICollection<PersistentClass> _nhibernatePersistentClass;
    private readonly SchemaDescription _defaultSchemaDescription;
    private readonly Dictionary<TableDescription, IList<PersistentClass>> _tableNameToPersistentClass;
    private IList<string> _removingSymbols = new[] {".[dbo]", "[", "]"};

    public ExceptionProcessingContext(ICollection<PersistentClass> nhibernatePersistentClass,
                                      SchemaDescription defaultSchemaDescription)
    {
        this._nhibernatePersistentClass = nhibernatePersistentClass;
        this._defaultSchemaDescription = defaultSchemaDescription;
        Func<Table, TableDescription> createTableDescriptionFunc = z => this.CreateTableDescription(z);

        this._tableNameToPersistentClass =
                nhibernatePersistentClass
                        .GroupBy(z=>z.Table)
                        .ToDictionary(z=>createTableDescriptionFunc(z.Key), z => (IList<PersistentClass>)z.ToList());
    }

    public TableDescription CreateTableDescription(Table table)
    {
        return this.CreateTableDescription(table.Catalog, table.Schema, table.Name);
    }

    public TableDescription CreateTableDescription(string initialCatalog, string schema, string tableName)
    {
        return new TableDescription(
                                    string.IsNullOrWhiteSpace(initialCatalog) ? this._defaultSchemaDescription.InitialCatalog : this.TryRemoveSymbols(initialCatalog),
                                    string.IsNullOrWhiteSpace(schema) ? string.Empty : this.TryRemoveSymbols(schema.ToLower()),
                                    this.TryRemoveSymbols(tableName.ToLower()));
    }

    public ICollection<PersistentClass> NhibernatePersistentClass
    {
        get { return this._nhibernatePersistentClass; }
    }

    public IEnumerable<PersistentClass> GetPersistentClass(TableDescription tableDescription)
    {
        IList<PersistentClass> result;
        if(!this._tableNameToPersistentClass.TryGetValue(tableDescription, out result))
        {
            //костыль
            var results = this._tableNameToPersistentClass.Where(z => string.Equals(z.Key.Name, tableDescription.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (results.Count == 1)
            {
                return results.First().Value;
            }

            throw new ArgumentNullException($"{tableDescription.ToString()} do not register in exception monitoring system");
        }

        return result;
    }

    private string TryRemoveSymbols(string value)
    {
        return this._removingSymbols.Aggregate(value, (prev, symbol) =>
                                                      {
                                                          var index = prev.IndexOf(symbol);
                                                          var length = symbol.Length;
                                                          if (-1 != index)
                                                          {
                                                              return prev.Remove(index, length);
                                                          }
                                                          return prev;
                                                      });
    }
    public struct TableDescription : IEquatable<TableDescription>
    {
        public string Schema { get; private set; }
        public string Catalog { get; private set; }
        public string Name { get; private set; }


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

        public override string ToString()
        {
            return string.Join(".", new[]{this.Catalog, this.Schema, this.Name}.Where(z=>null != z));
        }
        public override bool Equals(object obj)
        {
            return string.Equals(this.ToString(), obj.ToString());
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
