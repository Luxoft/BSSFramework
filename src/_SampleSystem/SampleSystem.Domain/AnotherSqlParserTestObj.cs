using Framework.DomainDriven.Attributes;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace SampleSystem.Domain;

[DomainType("{2328B905-DD6F-4304-A406-09A8D56A365F}")]
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
