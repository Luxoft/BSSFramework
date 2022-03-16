using Framework.Core;
using Framework.Persistent;
using Framework.Validation;

namespace AttachmentsSampleSystem.Domain
{
    [Framework.Restriction.UniqueGroup("Uni_Name")]
    [Framework.Restriction.UniqueGroup("Uni_Code")]
    [Framework.Restriction.UniqueGroup("Uni_CodeNative")]
    [Framework.Restriction.UniqueGroup("Uni_NameNative")]
    public abstract class HRDepartmentBase : BaseDirectory, ICodeObject
    {
        private string code;
        private string codeNative;
        private string nameNative;
        private Location location;
        private Employee head;
        private long externalId;
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

        [Framework.Restriction.UniqueElement("Uni_Name")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        [RequiredValidator(OperationContext = (int)(AttachmentsSampleSystemOperationContext.Request | AttachmentsSampleSystemOperationContext.Register))]
        [CustomName("Code in English")]
        [Framework.Restriction.MaxLength(50)]
        [Framework.Restriction.UniqueElement("Uni_Code")]
        public virtual string Code
        {
            get { return this.code.TrimNull(); }
            set { this.code = value.TrimNull(); }
        }

        [RequiredValidator(OperationContext = (int)(AttachmentsSampleSystemOperationContext.Request | AttachmentsSampleSystemOperationContext.Register))]
        [CustomName("Code in local language")]
        [Framework.Restriction.MaxLength(50)]
        [Framework.Restriction.UniqueElement("Uni_CodeNative")]
        public virtual string CodeNative
        {
            get { return this.codeNative.TrimNull(); }
            set { this.codeNative = value.TrimNull(); }
        }

        [RequiredValidator(OperationContext = (int)(AttachmentsSampleSystemOperationContext.Request | AttachmentsSampleSystemOperationContext.Register))]
        [CustomName("Name in local language")]

        [Framework.Restriction.UniqueElement("Uni_NameNative")]
        public virtual string NameNative
        {
            get { return this.nameNative.TrimNull(); }
            set { this.nameNative = value.TrimNull(); }
        }

        [Framework.Restriction.Required]
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
}
