using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Validation;

namespace SampleSystem.Domain;

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
    private Employee head;
    private long externalId;
    private CompanyLegalEntity companyLegalEntity;
    private bool isProduction;
    private bool isLegal;

    public virtual Location Location
    {
        get { return this.location; }
        set { this.location = value; }
    }

    public virtual bool IsProduction
    {
        get { return this.isProduction; }
        set { this.isProduction = value; }
    }

    public virtual bool IsLegal
    {
        get { return this.isLegal; }
        set { this.isLegal = value; }
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Luxoft Legal Entity")]
    [UniqueElement("Uni_Name")]
    [UniqueElement("Uni_NameNative")]
    public virtual CompanyLegalEntity CompanyLegalEntity
    {
        get { return this.companyLegalEntity; }
        set { this.companyLegalEntity = value; }
    }

    [UniqueElement("Uni_Name")]
    public override string Name
    {
        get { return base.Name; }
        set { base.Name = value; }
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Code in English")]
    [MaxLength(50)]
    [UniqueElement("Uni_Code")]
    public virtual string Code
    {
        get { return this.code.TrimNull(); }
        set { this.code = value.TrimNull(); }
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Code in local language")]
    [MaxLength(50)]
    [UniqueElement("Uni_CodeNative")]
    public virtual string CodeNative
    {
        get { return this.codeNative.TrimNull(); }
        set { this.codeNative = value.TrimNull(); }
    }

    [RequiredValidator(OperationContext = (int)(SampleSystemOperationContext.Request | SampleSystemOperationContext.Register))]
    [CustomName("Name in local language")]

    [UniqueElement("Uni_NameNative")]
    public virtual string NameNative
    {
        get { return this.nameNative.TrimNull(); }
        set { this.nameNative = value.TrimNull(); }
    }

    [Required]
    [CustomName("Head of Department")]
    public virtual Employee Head
    {
        get { return this.head; }
        set { this.head = value; }
    }

    public virtual long ExternalId
    {
        get { return this.externalId; }
        set { this.externalId = value; }
    }
}
