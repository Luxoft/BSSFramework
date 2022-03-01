using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain
{
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.CompanyLegalEntityView)]
    [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.CompanyLegalEntityEdit)]
    [UniqueGroup]
    public class CompanyLegalEntity :
        LegalEntityBase,
        IParentSource<CompanyLegalEntity>,
        ICodeObject
    {
        private string code;
        private CompanyLegalEntity parent;
        private CompanyLegalEntityType type;

        private TestObjForNested currentObj;

        public virtual TestObjForNested CurrentObj
        {
            get { return this.currentObj; }
            set { this.currentObj = value; }
        }

        [Framework.Restriction.Required]
        [Framework.Restriction.MaxLength(100)]
        public virtual string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        public virtual CompanyLegalEntity Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        public virtual CompanyLegalEntityType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        [CustomSerialization(CustomSerializationMode.Normal)]
        public override bool Active
        {
            get { return base.Active; }
            set { base.Active = value; }
        }
    }
}
