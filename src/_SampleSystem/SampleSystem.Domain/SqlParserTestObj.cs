using Framework.DomainDriven.BLL;
using Framework.Restriction;
using Framework.Transfering;

namespace SampleSystem.Domain;

[BLLViewRole(Max = MainDTOType.FullDTO)]
[BLLSaveRole]
[BLLRemoveRole]
[UniqueGroup]
public class SqlParserTestObj : AuditPersistentDomainObjectBase
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
