using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
[UniqueGroup]
public class CompanyLegalEntity : LegalEntityBase, ICodeObject
{
    private string code;
    private CompanyLegalEntity parent;
    private CompanyLegalEntityType type;

    private TestObjForNested currentObj;

    public virtual TestObjForNested CurrentObj
    {
        get => this.currentObj;
        set => this.currentObj = value;
    }

    [Required]
    [MaxLength(100)]
    public virtual string Code
    {
        get => this.code;
        set => this.code = value;
    }

    public virtual CompanyLegalEntity Parent
    {
        get => this.parent;
        set => this.parent = value;
    }

    public virtual CompanyLegalEntityType Type
    {
        get => this.type;
        set => this.type = value;
    }

    [CustomSerialization(CustomSerializationMode.Normal)]
    public override bool Active
    {
        get => base.Active;
        set => base.Active = value;
    }
}
