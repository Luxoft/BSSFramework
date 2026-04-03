using System.Diagnostics.CodeAnalysis;

using Framework.Application.Domain;
using Framework.BLL.Domain;
using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.Core;
using Framework.Relations;
using Framework.Restriction;
using Framework.Validation;

namespace SampleSystem.Domain;

[System.Diagnostics.DebuggerDisplay("{Code}-{GetProjectTypeName()}")]
public class Project :
        AuditPersistentDomainObjectBase,
        ICodeObject,
        IVisualIdentityObject,
        IDetail<BusinessUnit>
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
    protected DateTime? startDate;
    private string code;
    private BusinessUnit businessUnit;
    private DateTime? plannedEndDate;
    private DateTime? endDate;

    [MaxLength(80)]
    [RequiredValidator(OperationContext = (int)SampleSystemOperationContext.Save)]
    [VisualIdentity]
    public virtual string Code
    {
        get => this.code.TrimNull();
        set => this.code = value.TrimNull();
    }

    [RequiredValidator(OperationContext = (int)SampleSystemOperationContext.Save)]
    public virtual BusinessUnit BusinessUnit
    {
        get => this.businessUnit;
        set => this.businessUnit = value;
    }

    public virtual ProjectStatus ProjectStatus
    {
        get
        {
            if (this.EndDate == null)
            {
                return ProjectStatus.Active;
            }

            if (this.EndDate.Value >= this.GetCurrentDate())
            {
                return ProjectStatus.Active;
            }

            return ProjectStatus.Closed;
        }
    }

    [Required]
    public virtual DateTime? StartDate
    {
        get => this.startDate;
        set => this.SetValueSafe(v => v.startDate, value);
    }

    public virtual DateTime? EndDate
    {
        get => this.endDate;
        set => this.endDate = value;
    }

    [Required]
    public virtual DateTime? PlannedEndDate
    {
        get => this.plannedEndDate;
        set => this.plannedEndDate = value;
    }

    string IVisualIdentityObject.Name => this.Code;

    BusinessUnit IDetail<BusinessUnit>.Master => this.BusinessUnit;

    /// <summary>
    /// For notifiactions
    /// </summary>
    /// <returns></returns>
    public virtual string GetProjectTypeName() => throw new NotImplementedException();

    /// <summary>
    /// For Version
    /// </summary>
    /// <returns></returns>
    public virtual ProjectVersionType GetProjectVersionType() => throw new NotImplementedException();

    public override string ToString() => this.Code;

    /// <summary>
    /// For Version Hack!
    /// ProjectVersion must be generic!!!
    /// </summary>
    /// <returns></returns>
    public virtual FinancialProjectType? GetFinancialProjectType() => null;

    public virtual IEnumerable<Project> GetLinks() => throw new NotImplementedException();

    public virtual IEnumerable<Project> GetLinksWithoutFinancialProject() => throw new NotImplementedException();

    public virtual DateTime GetCurrentDate() => DateTime.Today;
}
