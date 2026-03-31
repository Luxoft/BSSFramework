using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.Restriction;

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
