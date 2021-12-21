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

using SampleSystem.Domain.Enums;
using SampleSystem.Domain.Inline;
using SampleSystem.Domain.Validators.Employee;

namespace SampleSystem.Domain
{
    [EmployeeValidator]
    [UniqueGroup(UseDbEvaluation = true)]
    [BLLViewRole(Max = MainDTOType.FullDTO)]
    [BLLSaveRole(SaveType = BLLSaveType.Both)]
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeeView)]
    [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.EmployeeEdit)]
    [BLLEventRole(Mode = EventRoleMode.Save)]
    [BLLIntegrationSaveRole]
    [DomainType("{AA46DA53-9B21-4DEC-9C70-720BDA1CB198}")]
    public partial class Employee :
        AuditPersistentDomainObjectBase,
        IMaster<EmployeePersonalCellPhone>,
        IMaster<EmployeeCellPhone>,
        IMaster<EmployeeToEmployeeLink>,
        IMaster<EmployeeAndEmployeeSpecializationLink>,
        IMaster<EmployeePhoto>,
        ISecurityContext,
        IEmployee
    {
        private readonly ICollection<EmployeePhoto> employeePhotos = new List<EmployeePhoto>();

        private readonly ICollection<EmployeeCellPhone> cellPhones = new List<EmployeeCellPhone>();

        private readonly ICollection<EmployeePersonalCellPhone> personalCellPhones = new List<EmployeePersonalCellPhone>();

        private readonly ICollection<EmployeeToEmployeeLink> employeeToEmployeeLinks = new List<EmployeeToEmployeeLink>();

        private readonly ICollection<EmployeeAndEmployeeSpecializationLink> specializations = new List<EmployeeAndEmployeeSpecializationLink>();

        private BusinessUnit coreBusinessUnit;
        private HRDepartment hRDepartment;
        private EmployeePosition position;
        private Employee ppm;

        private Period educationDuration;
        private Gender gender;

        private FioShort nameEng = new FioShort();
        private Fio nameNative = new Fio();
        private Fio nameRussian = new Fio();
        private EmployeeRegistrationType registrationType;

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

        // ReSharper disable once InconsistentNaming
        private bool canBePPM;

        private string cellPhone;
        private string personalCellPhone;
        private ManagementUnit managementUnit;
        private EmployeeRole role;
        private EmployeeRoleDegree roleDegree;
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


        [UniqueGroup]
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual IEnumerable<EmployeePhoto> EmployeePhotos
        {
            get { return this.employeePhotos; }
        }

        public virtual IEnumerable<EmployeeCellPhone> CellPhones
        {
            get { return this.cellPhones; }
        }

        public virtual IEnumerable<EmployeeToEmployeeLink> EmployeeToEmployeeLinks
        {
            get { return this.employeeToEmployeeLinks; }
        }

        [UniqueGroup]
        public virtual IEnumerable<EmployeeAndEmployeeSpecializationLink> Specializations
        {
            get { return this.specializations; }
        }

        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
        [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeePersonalCellPhoneView)]
        [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.EmployeePersonalCellPhoneEdit)]
        public virtual IEnumerable<EmployeePersonalCellPhone> PersonalCellPhones
        {
            get { return this.personalCellPhones; }
        }

        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual EmployeePhoto DefaultPhoto
        {
            get { return this.EmployeePhotos.SingleOrDefault(z => z.IsDefault); }
        }

        public virtual string AccountName
        {
            get { return this.Login.Split('\\').LastOrDefault().TrimNull(); }
        }

        public virtual string MailAccountName
        {
            get { return this.Email.Split('@').FirstOrDefault().TrimNull(); }
        }

        [FetchPath("EmployeeToEmployeeLinks.LinkedEmployee")]
        public virtual Employee PersonalAssistant
        {
            get
            {
                return
                    this.EmployeeToEmployeeLinks.FirstOrDefault(
                        x => x.EmployeeLinkType == EmployeeLinkType.PersonalAssistant).Maybe(x => x.LinkedEmployee);
            }
        }

        public virtual EmployeeRole Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        public virtual EmployeeRoleDegree RoleDegree
        {
            get { return this.roleDegree; }
            set { this.roleDegree = value; }
        }

        [Obsolete("#IAD-20612")]
        public virtual bool CanBePPM
        {
            get { return this.canBePPM; }
            set { this.canBePPM = value; }
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
        [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeeView)]
        [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.EmployeeEdit)]
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

        [SampleSystemCrypt]
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
        [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeePersonalCellPhoneView)]
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


        [ExpandPath("HRDepartment.Location.Code")]
        public virtual int? LocationCode
        {
            get { return this.Location?.Code; }
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

        public virtual ManagementUnit ManagementUnit
        {
            get { return this.managementUnit; }
            protected internal set { this.managementUnit = value; }
        }

        public virtual HRDepartment HRDepartment
        {
            get { return this.hRDepartment; }
            protected internal set { this.hRDepartment = value; }
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Event | DTORole.Integration)]
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client)]
        [ExpandPath("HRDepartment.CompanyLegalEntity")]
        [DetailRole(false)]
        public virtual CompanyLegalEntity CompanyLegalEntity
        {
            get { return this.HRDepartment.Maybe(x => x.CompanyLegalEntity); }
        }

        public virtual DateTime? LastActionDate
        {
            get { return this.lastActionDate; }
            set { this.lastActionDate = value; }
        }

        [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.EmployeePositionEdit)]
        [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeePositionView)]
        public virtual EmployeePosition Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        ////public virtual WorkplaceElement Workplace
        ////{
        ////    get { return this.workplace; }
        ////    set { this.workplace = value; }
        ////}

        public virtual EmployeeRegistrationType RegistrationType
        {
            get { return this.registrationType; }
            set { this.registrationType = value; }
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

        public virtual Gender Gender
        {
            get { return this.gender; }
            set { this.gender = value; }
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

        #region IMaster<EmployeePersonalCellPhone> Members

        ICollection<EmployeePersonalCellPhone> IMaster<EmployeePersonalCellPhone>.Details
        {
            get { return this.personalCellPhones; }
        }

        ICollection<EmployeeCellPhone> IMaster<EmployeeCellPhone>.Details
        {
            get { return this.cellPhones; }
        }

        ICollection<EmployeeToEmployeeLink> IMaster<EmployeeToEmployeeLink>.Details
        {
            get { return this.employeeToEmployeeLinks; }
        }

        #endregion

        ICollection<EmployeeAndEmployeeSpecializationLink> IMaster<EmployeeAndEmployeeSpecializationLink>.Details
        {
            get { return this.specializations; }
        }

        ICollection<EmployeePhoto> IMaster<EmployeePhoto>.Details
        {
            get { return this.employeePhotos; }
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

        public virtual int? GetPin()
        {
            return this.Pin;
        }

        public override string ToString()
        {
            return this.Pin + " - " + this.NameNative;
        }
    }
}
