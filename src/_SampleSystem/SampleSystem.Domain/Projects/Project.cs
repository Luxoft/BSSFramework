using System.Diagnostics.CodeAnalysis;

using Framework.Core;
using Framework.Persistent;
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
        get { return this.code.TrimNull(); }
        set { this.code = value.TrimNull(); }
    }

    [RequiredValidator(OperationContext = (int)SampleSystemOperationContext.Save)]
    public virtual BusinessUnit BusinessUnit
    {
        get { return this.businessUnit; }
        set { this.businessUnit = value; }
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

    [Framework.Restriction.Required]
    public virtual DateTime? StartDate
    {
        get { return this.startDate; }
        set { this.SetValueSafe(v => v.startDate, value); }
    }

    public virtual DateTime? EndDate
    {
        get { return this.endDate; }
        set { this.endDate = value; }
    }

    [Framework.Restriction.Required]
    public virtual DateTime? PlannedEndDate
    {
        get { return this.plannedEndDate; }
        set { this.plannedEndDate = value; }
    }

    string IVisualIdentityObject.Name
    {
        get { return this.Code; }
    }

    BusinessUnit IDetail<BusinessUnit>.Master
    {
        get { return this.BusinessUnit; }
    }

    /// <summary>
    /// For notifiactions
    /// </summary>
    /// <returns></returns>
    public virtual string GetProjectTypeName()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// For Version
    /// </summary>
    /// <returns></returns>
    public virtual ProjectVersionType GetProjectVersionType()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return this.Code;
    }

    /// <summary>
    /// For Version Hack!
    /// ProjectVersion must be generic!!!
    /// </summary>
    /// <returns></returns>
    public virtual FinancialProjectType? GetFinancialProjectType()
    {
        return null;
    }

    public virtual IEnumerable<Project> GetLinks()
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<Project> GetLinksWithoutFinancialProject()
    {
        throw new NotImplementedException();
    }

    public virtual DateTime GetCurrentDate()
    {
        return DateTime.Today;
    }
}
