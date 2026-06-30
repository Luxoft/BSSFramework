using Framework.Database.Mapping;
using Framework.Restriction;

namespace SampleSystem.Domain;

[UniqueGroup]
[Table(Name = "SqlParserTestObj")]
[NotAuditedClass]
public class AnotherSqlParserTestObj : AuditPersistentDomainObjectBase
{
    private string notNullColumn = null!;

    private string uniqueColumn = null!;

    public virtual string NotNullColumn
    {
        get => this.notNullColumn;
        set => this.notNullColumn = value;
    }

    [UniqueElement]
    public virtual string UniqueColumn
    {
        get => this.uniqueColumn;
        set => this.uniqueColumn = value;
    }
}

