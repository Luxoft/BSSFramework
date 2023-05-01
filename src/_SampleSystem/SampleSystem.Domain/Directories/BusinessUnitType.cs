using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

using JetBrains.Annotations;

namespace SampleSystem.Domain;

public interface ICreator
{
    static abstract void Create();
}

[BLLViewRole, BLLSaveRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.BusinessUnitTypeView)]
[SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.BusinessUnitTypeEdit)]
[UniqueGroup]
public partial class BusinessUnitType :
    BaseDirectory,
    IMaster<BusinessUnitTypeLinkWithPossibleParent>,
    IMaster<BusinessUnitTypeLinkWithPossibleFinancialProjectType>,
    IMaster<BusinessUnitTypeLinkWithTransferTo>,
    ICreator
{
    public static readonly Guid AccountTypeId = new Guid("E186F760-1BDE-4C95-8423-6C3CD2AFB4BF");

    public static readonly Guid LobTypeId = new Guid("F513D669-5CB7-4C00-A1FE-941993C062FE");

    public static readonly Guid ServiceTypeId = new Guid("410A7481-BDE9-413D-A430-FE2B0EC21805");

    public static readonly Guid DivisionTypeId = new Guid("C746C9B1-7249-48EC-AA9B-5176787FB4EC");

    public static readonly Guid ProgramTypeId = new Guid("0BDAC72D-2A31-438B-AA0A-82BB1FFE94EF");

    public static readonly Guid SEAdministrativeId = new Guid("0436552c-3028-44ba-84e4-a171013c4686");

    private readonly ICollection<BusinessUnitTypeLinkWithPossibleParent> possibleParents =
        new List<BusinessUnitTypeLinkWithPossibleParent>();

    private readonly ICollection<BusinessUnitTypeLinkWithTransferTo> transferTo =
        new List<BusinessUnitTypeLinkWithTransferTo>();

    private readonly ICollection<BusinessUnitTypeLinkWithPossibleFinancialProjectType> possibleFinancialProjectTypes =
        new List<BusinessUnitTypeLinkWithPossibleFinancialProjectType>();

    private bool canBeLinkedToDepartment;

    private bool canBeResourcePool;

    private bool projectStartAllowed;

    private bool startBOConfirm;

    private PossibleStartDate? possibleStartDate;

    private bool additionalStartConfirm;

    private bool additionalTransferConfirm;

    private bool transferBOConfirm;

    private PossibleStartDate? possibleTransferDate;

    private bool isAdministrative;

    private bool billingProjectAreNotAllowed;

    private bool needVertical;

    private bool canBeNewBusiness;

    private bool canBeIsSpecialCommission;

    private bool practiceAllowed;

    private bool canBeLinkedToClient;

    public BusinessUnitType()
    {
    }

    public BusinessUnitType([NotNull] string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        this.Name = name;
    }

    public virtual PossibleStartDate? PossibleStartDate { get { return this.possibleStartDate; } set { this.possibleStartDate = value; } }

    public virtual PossibleStartDate? PossibleTransferDate
    {
        get { return this.possibleTransferDate; }
        set { this.possibleTransferDate = value; }
    }

    public virtual bool IsAdministrative { get { return this.isAdministrative; } set { this.isAdministrative = value; } }

    public virtual bool CanBeLinkedToDepartment
    {
        get { return this.canBeLinkedToDepartment; }
        set { this.canBeLinkedToDepartment = value; }
    }

    public virtual bool NeedVertical { get { return this.needVertical; } set { this.needVertical = value; } }

    public virtual bool CanBeResourcePool { get { return this.canBeResourcePool; } set { this.canBeResourcePool = value; } }

    public virtual bool PracticeAllowed { get { return this.practiceAllowed; } set { this.practiceAllowed = value; } }

    public virtual bool CanBeNewBusiness { get { return this.canBeNewBusiness; } set { this.canBeNewBusiness = value; } }

    public virtual bool CanBeIsSpecialCommission
    {
        get { return this.canBeIsSpecialCommission; }
        set { this.canBeIsSpecialCommission = value; }
    }

    public virtual bool BillingProjectAreNotAllowed
    {
        get { return this.billingProjectAreNotAllowed; }
        set { this.billingProjectAreNotAllowed = value; }
    }

    public virtual bool ProjectStartAllowed { get { return this.projectStartAllowed; } set { this.projectStartAllowed = value; } }

    public virtual bool StartBOConfirm { get { return this.startBOConfirm; } set { this.startBOConfirm = value; } }

    public virtual bool TransferBOConfirm { get { return this.transferBOConfirm; } set { this.transferBOConfirm = value; } }

    public virtual bool AdditionalStartConfirm { get { return this.additionalStartConfirm; } set { this.additionalStartConfirm = value; } }

    public virtual bool AdditionalTransferConfirm
    {
        get { return this.additionalTransferConfirm; }
        set { this.additionalTransferConfirm = value; }
    }

    public virtual bool CanBeLinkedToClient { get { return this.canBeLinkedToClient; } set { this.canBeLinkedToClient = value; } }

    public virtual IEnumerable<BusinessUnitTypeLinkWithPossibleParent> PossibleParents { get { return this.possibleParents; } }

    public virtual IEnumerable<BusinessUnitTypeLinkWithTransferTo> TransferTo { get { return this.transferTo; } }

    public virtual IEnumerable<BusinessUnitTypeLinkWithPossibleFinancialProjectType> PossibleFinancialProjectTypes
    {
        get { return this.possibleFinancialProjectTypes; }
    }

    ICollection<BusinessUnitTypeLinkWithPossibleParent> IMaster<BusinessUnitTypeLinkWithPossibleParent>.Details
    {
        get { return (ICollection<BusinessUnitTypeLinkWithPossibleParent>)this.PossibleParents; }
    }

    ICollection<BusinessUnitTypeLinkWithTransferTo> IMaster<BusinessUnitTypeLinkWithTransferTo>.Details
    {
        get { return (ICollection<BusinessUnitTypeLinkWithTransferTo>)this.TransferTo; }
    }

    ICollection<BusinessUnitTypeLinkWithPossibleFinancialProjectType> IMaster<BusinessUnitTypeLinkWithPossibleFinancialProjectType>.Details
    {
        get { return (ICollection<BusinessUnitTypeLinkWithPossibleFinancialProjectType>)this.PossibleFinancialProjectTypes; }
    }

    public static void Create()
    {
        // Actively doing nothin
    }
}
