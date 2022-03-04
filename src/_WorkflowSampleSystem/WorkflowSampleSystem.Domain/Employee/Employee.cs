using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.SecuritySystem;
using Framework.Transfering;
using Framework.Validation;

using WorkflowSampleSystem.Domain.Inline;

namespace WorkflowSampleSystem.Domain
{
    [UniqueGroup(UseDbEvaluation = true)]
    [BLLViewRole(Max = MainDTOType.FullDTO)]
    [BLLSaveRole(SaveType = BLLSaveType.Both)]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeView)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeEdit)]
    [BLLEventRole(Mode = EventRoleMode.Save)]
    [BLLIntegrationSaveRole]
    [DomainType("{AA46DA53-9B21-4DEC-9C70-720BDA1CB198}")]
    public partial class Employee :
        AuditPersistentDomainObjectBase,
        IEmployee
    {
        private BusinessUnit coreBusinessUnit;
        private HRDepartment hRDepartment;
        private Employee ppm;

        private Period educationDuration;

        private FioShort nameEng = new FioShort();
        private Fio nameNative = new Fio();
        private Fio nameRussian = new Fio();

        private string email;
        private string login;
        private int? pin;
        private long externalId;
        private DateTime? plannedHireDate;
        private DateTime? hireDate;
        private DateTime? dismissDate;
        private DateTime? birthDate;
        private DateTime? lastActionDate;
        private string interphone;
        private string landlinephone;
        private Employee vacationApprover;

        private string cellPhone;
        private string personalCellPhone;
        private Period workPeriod;

        private int age;

        [NotPersistentField]
        private DateTime nonValidateVirtualField = DateTime.Now;

        [NotPersistentField]
        private DateTime validateVirtualField = DateTime.Now;

        public virtual DateTime NonValidateVirtualProp
        {
            get { return this.nonValidateVirtualField; }
            set { this.nonValidateVirtualField = value; }
        }


        [PropertyValidationMode(true)]
        public virtual DateTime ValidateVirtualProp
        {
            get { return this.validateVirtualField; }
            set { this.validateVirtualField = value; }
        }

        public virtual string AccountName
        {
            get { return this.Login.Split('\\').LastOrDefault().TrimNull(); }
        }

        public virtual string MailAccountName
        {
            get { return this.Email.Split('@').FirstOrDefault().TrimNull(); }
        }

        public virtual long ExternalId
        {
            get { return this.externalId; }
            set { this.externalId = value; }
        }

        public virtual DateTime? PlannedHireDate
        {
            get { return this.plannedHireDate; }
            set { this.plannedHireDate = value; }
        }

        [MaxLength(50)]
        public virtual string Email
        {
            get { return this.email.TrimNull(); }
            set { this.email = value.TrimNull(); }
        }

        [MaxLength(30)]
        [UniqueElement]
        [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeView)]
        [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeEdit)]
        public virtual string Login
        {
            get { return this.login.TrimNull(); }
            set { this.login = value.TrimNull(); }
        }

        [MaxLength(25)]
        public virtual string Interphone
        {
            get { return this.interphone.TrimNull(); }
            set { this.interphone = value.TrimNull(); }
        }

        [PropertyValidationMode(false)]
        public virtual string CellPhone
        {
            get { return this.cellPhone.TrimNull(); }
            protected internal set { this.cellPhone = value.TrimNull(); }
        }

        [MaxLength(40)]
        public virtual string Landlinephone
        {
            get { return this.landlinephone.TrimNull(); }
            set { this.landlinephone = value.TrimNull(); }
        }

        [WorkflowSampleSystemCrypt]
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
        public virtual string PersonalCellPhone
        {
            get { return this.personalCellPhone.TrimNull(); }
            protected internal set { this.personalCellPhone = value.TrimNull(); }
        }

        public virtual bool IsCandidate
        {
            get { return this.Pin.GetValueOrDefault(0) == 0; }
        }

        public virtual int? Pin
        {
            get { return this.pin; }
            set { this.pin = value; }
        }

        public virtual FioShort NameEng
        {
            get { return this.nameEng; }
            set { this.nameEng = value; }
        }

        public virtual Fio NameNative
        {
            get { return this.nameNative; }
            set { this.nameNative = value; }
        }

        public virtual Fio NameRussian
        {
            get { return this.nameRussian; }
            set { this.nameRussian = value; }
        }

        public virtual DateTime? DismissDate
        {
            get { return this.dismissDate; }
            protected internal set { this.dismissDate = value; }
        }

        public virtual DateTime? BirthDate
        {
            get { return this.birthDate; }
            set { this.birthDate = value; }
        }

        public virtual DateTime? HireDate
        {
            get { return this.hireDate; }
            protected internal set { this.hireDate = value; }
        }

        ////public virtual WorkplaceOfficeAddress Address
        ////{
        ////    get { return this.address; }
        ////    set { this.address = value; }
        ////}

        [ExpandPath("HRDepartment.Location")]
        public virtual Location Location
        {
            get { return this.HRDepartment?.Location; }
        }

        public virtual BusinessUnit CoreBusinessUnit
        {
            get { return this.coreBusinessUnit; }
            protected internal set { this.coreBusinessUnit = value; }
        }

        [ExpandPath("CoreBusinessUnit.Period")]
        public virtual Period? CoreBusinessUnitPeriod
        {
            get { return this.CoreBusinessUnit?.Period; }
        }

        public virtual HRDepartment HRDepartment
        {
            get { return this.hRDepartment; }
            protected internal set { this.hRDepartment = value; }
        }

        public virtual DateTime? LastActionDate
        {
            get { return this.lastActionDate; }
            set { this.lastActionDate = value; }
        }

        [Obsolete("#IAD-20612")]
        public virtual Employee Ppm
        {
            get { return this.ppm; }
            set { this.ppm = value; }
        }

        [Obsolete("#IAD-20612")]
        public virtual Employee VacationApprover
        {
            get { return this.vacationApprover; }
            set { this.vacationApprover = value; }
        }

        public virtual Period EducationDuration
        {
            get { return this.educationDuration; }
            set { this.educationDuration = value; }
        }

        public virtual Period WorkPeriod
        {
            get { return this.workPeriod; }
            set { this.workPeriod = value; }
        }

        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client)]
        public virtual string LogonName
        {
            get
            {
                var parts = this.Login.Split('\\');
                var domain = parts.Length == 2 ? parts[0].Trim() : string.Empty;
                var accountName = parts.Length == 2 ? parts[1] : parts[0];
                return accountName + "@" + domain +
                       (domain.Equals("luxoft", StringComparison.CurrentCultureIgnoreCase) ? ".com" : ".luxoft.com");
            }
        }

        public virtual int Age
        {
            get
            {
                return this.age;}
            set
            {
                this.age = value;
            }
        }

        public override string ToString()
        {
            return this.Pin + " - " + this.NameNative;
        }
    }
}
