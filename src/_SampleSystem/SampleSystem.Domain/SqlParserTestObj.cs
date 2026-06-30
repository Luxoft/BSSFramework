using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole(Max = MainDTOType.FullDTO)]
[BLLSaveRole]
[BLLRemoveRole]
[UniqueGroup]
public class SqlParserTestObj : AuditPersistentDomainObjectBase
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

