using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.NHibernate.Audit;

using NHibernate.Dialect;
using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace Framework.DomainDriven.NHibernate;

class AuditTableGenerateContext
{
    public AuditTableGenerateContext(ITableMetadata originalTable, ITableMetadata tableInfo, Table table, Dialect dialect, IMapping mapping, string defaultCatalog, string defaultSchema, PersistentClass original, IAuditAttributeService auditAttributeService, IDatabaseScriptGeneratorContext context)
    {
        this.Context = context;
        this.OriginalTableMetadata = originalTable;

        this.Dialect = dialect;
        this.Mapping = mapping;
        this.DefaultCatalog = defaultCatalog;
        this.DefaultSchema = defaultSchema;
        this.Original = original;
        this.AuditAttributeService = auditAttributeService;
        this.Table = table;
        this.TableInfo = tableInfo;
    }

    public Dialect Dialect { get; }

    public IMapping Mapping { get; }

    public string DefaultCatalog { get; }

    public Table Table { get; }

    public ITableMetadata TableInfo { get; }

    public string DefaultSchema { get; }

    public ITableMetadata OriginalTableMetadata { get; }

    public PersistentClass Original { get; }

    public IAuditAttributeService AuditAttributeService { get; }

    public IDatabaseScriptGeneratorContext Context { get; }

    public string GetQualifiedTableName()
    {
        if (!string.IsNullOrEmpty(this.Table.Subselect))
        {
            return "( " + this.Table.Subselect + " )";
        }
        string quotedName = this.GetQuotedName(this.Table.Name);
        string usedSchema = this.Table.Schema == null ? this.DefaultSchema : this.GetQuotedName(this.Table.Schema);
        string usedCatalog = this.Table.Catalog ?? this.DefaultCatalog;
        return this.Dialect.Qualify(usedCatalog, usedSchema, quotedName);
    }

    private string GetQuotedName(string value)
    {
        return this.Table.IsQuoted ? this.Dialect.QuoteForTableName(value) : value;
    }

}
