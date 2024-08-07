using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace SampleSystem.Domain;

[UniqueGroup]
[Table(Name = "SqlParserTestObj")]
[NotAuditedClass]
public class AnotherSqlParserTestObj : AuditPersistentDomainObjectBase
{
    private string notNullColumn;

    private string uniqueColumn;

    public virtual string NotNullColumn
    {
        get { return this.notNullColumn; }
        set { this.notNullColumn = value; }
    }

    [UniqueElement]
    public virtual string UniqueColumn
    {
        get { return this.uniqueColumn; }
        set { this.uniqueColumn = value; }
    }
}
