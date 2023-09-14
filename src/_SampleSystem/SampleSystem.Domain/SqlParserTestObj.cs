using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;
using Framework.Transfering;

namespace SampleSystem.Domain;

[DomainType("{4963D86E-5650-41E0-BDBA-0274FF2CF375}")]
[BLLViewRole(Max = MainDTOType.FullDTO)]
[BLLSaveRole]
[BLLRemoveRole]
[UniqueGroup]
[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.Disabled))]
[EditDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.Disabled))]
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
