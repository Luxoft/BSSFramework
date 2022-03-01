using System;

using Framework.Persistent;

namespace SampleSystem.Domain
{
    public class BusinessUnitTypeLinkWithPossibleParent :
        AuditPersistentDomainObjectBase,
        IDetail<BusinessUnitType>
    {
        private BusinessUnitType businessUnitType;

        private BusinessUnitType possibleParent;

        public BusinessUnitTypeLinkWithPossibleParent()
        {
        }

        public BusinessUnitTypeLinkWithPossibleParent(BusinessUnitType businessUnitType)
        {
            if (businessUnitType == null)
            {
                throw new ArgumentNullException(nameof(businessUnitType));
            }

            this.businessUnitType = businessUnitType;
            this.businessUnitType.AddDetail(this);
        }

        [IsMaster]
        public virtual BusinessUnitType BusinessUnitType
        {
            get
            {
                return this.businessUnitType;
            }

            set
            {
                this.businessUnitType = value;
            }
        }

        public virtual BusinessUnitType PossibleParent
        {
            get
            {
                return this.possibleParent;
            }

            set
            {
                this.possibleParent = value;
            }
        }

        BusinessUnitType IDetail<BusinessUnitType>.Master
        {
            get
            {
                return this.businessUnitType;
            }
        }
    }
}
