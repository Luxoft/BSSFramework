using Framework.Database.NHibernate.Audit;
using Framework.Database.NHibernate.DBGenerator.Contracts;

using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace Framework.Database.NHibernate.DBGenerator.AuditDBGenerator;

class AuditTableGenerateContext(
    ITableMetadata originalTable,
    ITableMetadata tableInfo,
    Table table,
    global::NHibernate.Dialect.Dialect dialect,
    IMapping mapping,
    string defaultCatalog,
    string defaultSchema,
    PersistentClass original,
    IAuditAttributeService auditAttributeService,
    IDatabaseScriptGeneratorContext context)
{
    public global::NHibernate.Dialect.Dialect Dialect { get; } = dialect;

    public IMapping Mapping { get; } = mapping;

    public string DefaultCatalog { get; } = defaultCatalog;

    public Table Table { get; } = table;

    public ITableMetadata TableInfo { get; } = tableInfo;

    public string DefaultSchema { get; } = defaultSchema;

    public ITableMetadata OriginalTableMetadata { get; } = originalTable;

    public PersistentClass Original { get; } = original;

    public IAuditAttributeService AuditAttributeService { get; } = auditAttributeService;

    public IDatabaseScriptGeneratorContext Context { get; } = context;

    public string GetQualifiedTableName()
    {
        if (!string.IsNullOrEmpty(this.Table.Subselect))
        {
            return "( " + this.Table.Subselect + " )";
        }
        var quotedName = this.GetQuotedName(this.Table.Name);
        var usedSchema = this.Table.Schema == null ? this.DefaultSchema : this.GetQuotedName(this.Table.Schema);
        var usedCatalog = this.Table.Catalog ?? this.DefaultCatalog;
        return this.Dialect.Qualify(usedCatalog, usedSchema, quotedName);
    }

    private string GetQuotedName(string value) => this.Table.IsQuoted ? this.Dialect.QuoteForTableName(value) : value;
}
