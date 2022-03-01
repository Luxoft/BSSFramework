using System;

using Framework.Core;
using Framework.Persistent;
using Framework.Validation;

namespace SampleSystem.Domain
{
    public class EmployeeCellPhoneBase :
        AuditPersistentDomainObjectBase,
        IDetail<Employee>,
        INumberObject<string>
    {
        protected readonly Employee employee;

        protected string countryCode;
        protected string cityCode;
        protected string number;

        protected string fullNumber;

        protected EmployeeCellPhoneBase()
        {
        }

        protected EmployeeCellPhoneBase(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            this.employee = employee;
        }

        public virtual Employee Employee
        {
            get { return this.employee; }
        }

        [Framework.Restriction.Required]
        [NumberAlphabetValidator]
        [Framework.Restriction.MaxLength(3)]
        public virtual string CountryCode
        {
            get { return this.countryCode.TrimNull(); }
            set { this.countryCode = value.TrimNull(); }
        }

        [Framework.Restriction.Required]
        [NumberAlphabetValidator]
        [Framework.Restriction.MaxLength(5)]
        public virtual string CityCode
        {
            get { return this.cityCode.TrimNull(); }
            set { this.cityCode = value.TrimNull(); }
        }

        [Framework.Restriction.Required]
        [NumberAlphabetValidator]
        [Framework.Restriction.MaxLength(7)]
        public virtual string Number
        {
            get { return this.number.TrimNull(); }
            set { this.number = value.TrimNull(); }
        }

        [Framework.Restriction.Required]
        [NumberAlphabetValidator(ExternalChars = "+()")]
        [Framework.Restriction.MaxLength(18)]
        public virtual string FullNumber
        {
            get { return this.fullNumber.TrimNull(); }
            protected internal set { this.fullNumber = value.TrimNull(); }
        }

        Employee IDetail<Employee>.Master
        {
            get { return this.employee; }
        }
    }
}
