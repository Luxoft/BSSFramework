using Framework.DomainDriven.BLL;
using Framework.Restriction;
using Framework.Transfering;

namespace SampleSystem.Domain;

[BLLViewRole(Max = MainDTOType.FullDTO)]
[BLLSaveRole]
[UniqueGroup]
public class SqlParserTestObjContainer : AuditPersistentDomainObjectBase
{
    private SqlParserTestObj includedObject;

    public virtual SqlParserTestObj IncludedObject
    {
        get { return this.includedObject; }
        set { this.includedObject = value; }
    }
}
