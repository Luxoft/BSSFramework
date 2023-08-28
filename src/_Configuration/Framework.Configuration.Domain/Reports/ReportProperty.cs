using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain.Reports;

[BLLViewRole]
[ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.Disabled)]
public class ReportProperty : AuditPersistentDomainObjectBase, IDetail<Report>
{
    private Report report;
    private string propertyPath;
    private int order;
    private string alias;
    private int sortOrdered;
    private int sortType;
    private string formula;

    public ReportProperty(Report report)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));

        this.report = report;
        this.report.AddDetail(this);
    }

    protected internal ReportProperty()
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
    public virtual string PropertyPath
    {
        get { return this.propertyPath; }
        set { this.propertyPath = value; }
    }

    public virtual int Order
    {
        get { return this.order; }
        set { this.order = value; }
    }

    [Required]
    public virtual string Alias
    {
        get { return this.alias; }
        set { this.alias = value; }
    }

    public virtual int SortOrdered
    {
        get { return this.sortOrdered; }
        set { this.sortOrdered = value; }
    }

    public virtual int SortType
    {
        get { return this.sortType; }
        set { this.sortType = value; }
    }

    public virtual string Formula
    {
        get { return this.formula; }
        set { this.formula = value; }
    }
}
