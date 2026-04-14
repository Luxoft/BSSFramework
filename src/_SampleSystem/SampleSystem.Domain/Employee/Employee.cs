using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Fetching;
using Framework.BLL.Domain.Persistent.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Database.Mapping;
using Framework.Relations;
using Framework.Restriction;
using Framework.Validation;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Employee.EmpoloyeeLink;
using SampleSystem.Domain.Enums;
using SampleSystem.Domain.Inline;
using SampleSystem.Domain.MU;

using SecuritySystem;

namespace SampleSystem.Domain.Employee;

[UniqueGroup(UseDbEvaluation = true)]
[BLLViewRole(Max = MainDTOType.FullDTO)]
[BLLSaveRole(SaveType = BLLSaveType.Both)]
[BLLIntegrationSaveRole]
public partial class Employee :
        AuditPersistentDomainObjectBase,
        IMaster<EmployeePersonalCellPhone>,
        IMaster<EmployeeCellPhone>,
        IMaster<EmployeeToEmployeeLink>,
        IMaster<EmployeeAndEmployeeSpecializationLink>,
        IMaster<EmployeePhoto>,
        ISecurityContext//,
        //IEmployee
{
    private readonly ICollection<EmployeePhoto> employeePhotos = new List<EmployeePhoto>();

    private readonly ICollection<EmployeeCellPhone> cellPhones = new List<EmployeeCellPhone>();

    private readonly ICollection<EmployeePersonalCellPhone> personalCellPhones = new List<EmployeePersonalCellPhone>();

    private readonly ICollection<EmployeeToEmployeeLink> employeeToEmployeeLinks = new List<EmployeeToEmployeeLink>();

    private readonly ICollection<EmployeeAndEmployeeSpecializationLink> specializations = new List<EmployeeAndEmployeeSpecializationLink>();

    private BusinessUnit? coreBusinessUnit;
    private HRDepartment.HRDepartment? hRDepartment;
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
        get => this.nonValidateVirtualField;
        set => this.nonValidateVirtualField = value;
    }


    [PropertyValidationMode(true)]
    public virtual DateTime ValidateVirtualProp
    {
        get => this.validateVirtualField;
        set => this.validateVirtualField = value;
    }


    [UniqueGroup]
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual ICollection<EmployeePhoto> EmployeePhotos => this.employeePhotos;

    public virtual ICollection<EmployeeCellPhone> CellPhones => this.cellPhones;

    public virtual IEnumerable<EmployeeToEmployeeLink> EmployeeToEmployeeLinks => this.employeeToEmployeeLinks;

    [UniqueGroup]
    public virtual IEnumerable<EmployeeAndEmployeeSpecializationLink> Specializations => this.specializations;

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
    public virtual IEnumerable<EmployeePersonalCellPhone> PersonalCellPhones => this.personalCellPhones;

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual EmployeePhoto DefaultPhoto => this.EmployeePhotos.SingleOrDefault(z => z.IsDefault);

    public virtual string AccountName => this.Login.Split('\\').LastOrDefault().TrimNull();

    public virtual string MailAccountName => this.Email.Split('@').FirstOrDefault().TrimNull();

    [FetchPath("EmployeeToEmployeeLinks.LinkedEmployee")]
    public virtual Employee PersonalAssistant =>
        this.EmployeeToEmployeeLinks.FirstOrDefault(
            x => x.EmployeeLinkType == EmployeeLinkType.PersonalAssistant).Maybe(x => x.LinkedEmployee);

    public virtual EmployeeRole Role
    {
        get => this.role;
        set => this.role = value;
    }

    public virtual EmployeeRoleDegree RoleDegree
    {
        get => this.roleDegree;
        set => this.roleDegree = value;
    }

    [Obsolete("#IAD-20612")]
    public virtual bool CanBePPM
    {
        get => this.canBePPM;
        set => this.canBePPM = value;
    }

    public virtual long ExternalId
    {
        get => this.externalId;
        set => this.externalId = value;
    }

    public virtual DateTime? PlannedHireDate
    {
        get => this.plannedHireDate;
        set => this.plannedHireDate = value;
    }

    [MaxLength(50)]
    public virtual string Email
    {
        get => this.email.TrimNull();
        set => this.email = value.TrimNull();
    }

    [MaxLength(30)]
    [UniqueElement]
    public virtual string Login
    {
        get => this.login.TrimNull();
        set => this.login = value.TrimNull();
    }

    [MaxLength(25)]
    public virtual string Interphone
    {
        get => this.interphone.TrimNull();
        set => this.interphone = value.TrimNull();
    }

    [PropertyValidationMode(false)]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual string CellPhone
    {
        get => this.cellPhone.TrimNull();
        set => this.cellPhone = value.TrimNull();
    }

    [MaxLength(40)]
    public virtual string Landlinephone
    {
        get => this.landlinephone.TrimNull();
        set => this.landlinephone = value.TrimNull();
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client)]
    public virtual string PersonalCellPhone
    {
        get => this.personalCellPhone.TrimNull();
        set => this.personalCellPhone = value.TrimNull();
    }

    public virtual bool IsCandidate => this.Pin.GetValueOrDefault(0) == 0;

    public virtual int? Pin
    {
        get => this.pin;
        set => this.pin = value;
    }

    public virtual FioShort NameEng
    {
        get => this.nameEng;
        set => this.nameEng = value;
    }

    public virtual Fio NameNative
    {
        get => this.nameNative;
        set => this.nameNative = value;
    }

    public virtual Fio NameRussian
    {
        get => this.nameRussian;
        set => this.nameRussian = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual DateTime? DismissDate
    {
        get => this.dismissDate;
        set => this.dismissDate = value;
    }

    public virtual DateTime? BirthDate
    {
        get => this.birthDate;
        set => this.birthDate = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual DateTime? HireDate
    {
        get => this.hireDate;
        set => this.hireDate = value;
    }

    ////public virtual WorkplaceOfficeAddress Address
    ////{
    ////    get { return this.address; }
    ////    set { this.address = value; }
    ////}

    [ExpandPath("HRDepartment.Location")]
    public virtual Location Location => this.HRDepartment?.Location;

    [ExpandPath("HRDepartment.Location.Code")]
    public virtual int? LocationCode => this.Location?.Code;

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual BusinessUnit? CoreBusinessUnit
    {
        get => this.coreBusinessUnit;
        set => this.coreBusinessUnit = value;
    }

    [ExpandPath("CoreBusinessUnit.Period")]
    public virtual Period? CoreBusinessUnitPeriod => this.CoreBusinessUnit?.Period;

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual ManagementUnit ManagementUnit
    {
        get => this.managementUnit;
        set => this.managementUnit = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual HRDepartment.HRDepartment? HRDepartment
    {
        get => this.hRDepartment;
        set => this.hRDepartment = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Event | DTORole.Integration)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client)]
    [ExpandPath("HRDepartment.CompanyLegalEntity")]
    [DetailRole(false)]
    public virtual CompanyLegalEntity CompanyLegalEntity => this.HRDepartment.Maybe(x => x.CompanyLegalEntity);

    public virtual DateTime? LastActionDate
    {
        get => this.lastActionDate;
        set => this.lastActionDate = value;
    }

    public virtual EmployeePosition Position
    {
        get => this.position;
        set => this.position = value;
    }

    ////public virtual WorkplaceElement Workplace
    ////{
    ////    get { return this.workplace; }
    ////    set { this.workplace = value; }
    ////}

    public virtual EmployeeRegistrationType RegistrationType
    {
        get => this.registrationType;
        set => this.registrationType = value;
    }

    [Obsolete("#IAD-20612")]
    public virtual Employee Ppm
    {
        get => this.ppm;
        set => this.ppm = value;
    }

    [Obsolete("#IAD-20612")]
    public virtual Employee VacationApprover
    {
        get => this.vacationApprover;
        set => this.vacationApprover = value;
    }

    public virtual Period EducationDuration
    {
        get => this.educationDuration;
        set => this.educationDuration = value;
    }

    public virtual Gender Gender
    {
        get => this.gender;
        set => this.gender = value;
    }

    public virtual Period WorkPeriod
    {
        get => this.workPeriod;
        set => this.workPeriod = value;
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

    ICollection<EmployeePersonalCellPhone> IMaster<EmployeePersonalCellPhone>.Details => this.personalCellPhones;

    ICollection<EmployeeCellPhone> IMaster<EmployeeCellPhone>.Details => this.cellPhones;

    ICollection<EmployeeToEmployeeLink> IMaster<EmployeeToEmployeeLink>.Details => this.employeeToEmployeeLinks;

    #endregion

    ICollection<EmployeeAndEmployeeSpecializationLink> IMaster<EmployeeAndEmployeeSpecializationLink>.Details => this.specializations;

    ICollection<EmployeePhoto> IMaster<EmployeePhoto>.Details => this.employeePhotos;

    public virtual int Age
    {
        get => this.age;
        set => this.age = value;
    }

    public virtual int? GetPin() => this.Pin;

    public override string ToString() => this.Pin + " - " + this.NameNative;
}
