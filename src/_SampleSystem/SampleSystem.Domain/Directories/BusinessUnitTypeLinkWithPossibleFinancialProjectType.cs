using System;

using Framework.Persistent;

namespace SampleSystem.Domain
{
    public class BusinessUnitTypeLinkWithPossibleFinancialProjectType :
        AuditPersistentDomainObjectBase,
        IDetail<BusinessUnitType>
    {
        private BusinessUnitType businessUnitType;

        private FinancialProjectType financialProjectType;

        public BusinessUnitTypeLinkWithPossibleFinancialProjectType()
        {
        }

        public BusinessUnitTypeLinkWithPossibleFinancialProjectType(BusinessUnitType businessUnitType)
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

        public virtual FinancialProjectType FinancialProjectType
        {
            get
            {
                return this.financialProjectType;
            }

            set
            {
                this.financialProjectType = value;
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
