using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.Core;
using Framework.Restriction;
using Framework.Validation;
using Framework.Validation.Attributes;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Validation;

namespace SampleSystem.Domain.HRDepartment;

[UniqueGroup("Uni_Name")]
[UniqueGroup("Uni_Code")]
[UniqueGroup("Uni_CodeNative")]
[UniqueGroup("Uni_NameNative")]
public abstract class HRDepartmentBase : BaseDirectory, IExternalSynchronizable, ICodeObject
{
    private string code;
    private string codeNative;
    private string nameNative;
    private Location location;
    private Employee.Employee head;
    private long externalId;
    private CompanyLegalEntity companyLegalEntity;
    private bool isProduction;
    private bool isLegal;

    public virtual Location Location
    {
        get => this.location;
        set => this.location = value;
    }

    public virtual bool IsProduction
    {
        get => this.isProduction;
        set => this.isProduction = value;
    }

    public virtual bool IsLegal
    {
        get => this.isLegal;
        set => this.isLegal = value;
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Luxoft Legal Entity")]
    [UniqueElement("Uni_Name")]
    [UniqueElement("Uni_NameNative")]
    public virtual CompanyLegalEntity CompanyLegalEntity
    {
        get => this.companyLegalEntity;
        set => this.companyLegalEntity = value;
    }

    [UniqueElement("Uni_Name")]
    public override string Name
    {
        get => base.Name;
        set => base.Name = value;
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Code in English")]
    [MaxLength(50)]
    [UniqueElement("Uni_Code")]
    public virtual string Code
    {
        get => this.code.TrimNull();
        set => this.code = value.TrimNull();
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Code in local language")]
    [MaxLength(50)]
    [UniqueElement("Uni_CodeNative")]
    public virtual string CodeNative
    {
        get => this.codeNative.TrimNull();
        set => this.codeNative = value.TrimNull();
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Name in local language")]

    [UniqueElement("Uni_NameNative")]
    public virtual string NameNative
    {
        get => this.nameNative.TrimNull();
        set => this.nameNative = value.TrimNull();
    }

    [Required]
    [CustomName("Head of Department")]
    public virtual Employee.Employee Head
    {
        get => this.head;
        set => this.head = value;
    }

    public virtual long ExternalId
    {
        get => this.externalId;
        set => this.externalId = value;
    }
}
