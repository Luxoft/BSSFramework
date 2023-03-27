using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

using JetBrains.Annotations;

namespace Framework.Configuration.Domain.Reports;

[NotAuditedClass]
[BLLViewRole]
[ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.Disabled)]
public class ReportFilter : AuditPersistentDomainObjectBase, IDetail<Report>
{
    public const string IsBeforeOrNullOperator = "isnullorlt";
    public const string IsAfterOrNullOperator = "isnullorgt";
    public const string IsNullOrBeforeOrEqualOperator = "isnullorlte";
    public const string IsNullOrAfterOrEqualOperator = "isnullorgte";

    private Report report;

    private string property;
    private string filterOperator;
    private string value;
    private bool isValueFromParameters;

    public ReportFilter([NotNull] Report report)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));
        this.report = report;
        this.report.AddDetail(this);
    }

    protected internal ReportFilter()
    {
    }

    [Required]
    public virtual Report Report
    {
        get { return this.report; }
    }

    Report IDetail<Report>.Master
    {
        get { return this.Report; }
    }

    [Required]
    public virtual string Property
    {
        get { return this.property; }
        set { this.property = value; }
    }

    [Required]
    public virtual string Value
    {
        get { return this.value; }
        set { this.value = value; }
    }

    [Required]
    public virtual string FilterOperator
    {
        get { return this.filterOperator; }
        set { this.filterOperator = value; }
    }

    public virtual bool IsValueFromParameters
    {
        get { return this.isValueFromParameters; }
        set { this.isValueFromParameters = value; }
    }

    public virtual string FilterOperatorViewName
    {
        get
        {
            if (string.IsNullOrEmpty(this.FilterOperator))
            {
                return string.Empty;
            }

            switch (this.FilterOperator.ToLower())
            {
                case "eq":
                    return "Is equal to";
                case "neq":
                    return "Is not equal to";
                case "startswith":
                    return "Starts with";
                case "contains":
                    return "Contains";
                case "doesnotcontain":
                    return "Does not contain";
                case "endswith":
                    return "Ends with";
                case "lte":
                    return "Is less than or equal to";
                case "gte":
                    return "Is after or equal to";
                case "gt":
                    return "Is after";
                case "lt":
                    return "Is before";
                case "isIntersectedP":
                    return "intersected";
                case "containsP":
                    return "contains";
                case IsBeforeOrNullOperator:
                    return "Is before or null";
                case IsAfterOrNullOperator:
                    return "Is after or null";
                case IsNullOrBeforeOrEqualOperator:
                    return "Is before or equal to or null";
                case IsNullOrAfterOrEqualOperator:
                    return "Is after or equal to or null";
            }

            return this.FilterOperator;
        }
    }
}
