using System;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;

using SampleSystem.Domain.Inline;

namespace SampleSystem.Domain
{
    public class InsuranceDetail : AuditPersistentDomainObjectBase
    {
        private decimal cost;

        private Fio fio;

        private DateTime? birthDate;

        private int age;

        private string landlinePhone;

        private string cellPhone;

        private string registrationAddress;

        private string residentalAddress;

        [Money]
        public virtual decimal Cost
        {
            get { return this.cost.RoundMoney(); }
            set { this.cost = value.RoundMoney(); }
        }

        public virtual Fio Fio
        {
            get { return this.fio; }
            set { this.fio = value; }
        }

        public virtual DateTime? BirthDate
        {
            get { return this.birthDate; }
            set { this.birthDate = value; }
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual int Age
        {
            get { return this.age; }
            set { this.age = value; }
        }

        public virtual string LandlinePhone
        {
            get { return this.landlinePhone; }
            set { this.landlinePhone = value; }
        }

        public virtual string CellPhone
        {
            get { return this.cellPhone; }
            set { this.cellPhone = value; }
        }

        public virtual string RegistrationAddress
        {
            get { return this.registrationAddress; }
            set { this.registrationAddress = value; }
        }

        public virtual string ResidentalAddress
        {
            get { return this.residentalAddress; }
            set { this.residentalAddress = value; }
        }
    }
}
