using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.Security;

namespace Framework.Configuration.Domain.Reports;

[UniqueGroup]
[NotAuditedClass]
[BLLViewRole]
[ViewDomainObject(typeof(ConfigurationSecurityOperation), nameof(ConfigurationSecurityOperation.Disabled))]
public class ReportParameter : AuditPersistentDomainObjectBase, IDetail<Report>
{
    private Report report;

    private string typeName;

    private string name;

    private bool isRequired;

    private int order;

    private string displayValueProperty;

    private bool isCollection;

    public ReportParameter(Report report)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));

        this.report = report;
        this.report.AddDetail(this);
    }

    protected internal ReportParameter()
    {
    }

    [UniqueElement]
    public virtual Report Report
    {
        get { return this.report; }
    }

    [UniqueElement]
    public virtual string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public virtual string TypeName
    {
        get { return this.typeName; }
        set { this.typeName = value; }
    }

    public virtual bool IsCollection
    {
        get { return this.isCollection; }
        set { this.isCollection = value; }
    }

    public virtual bool IsRequired
    {
        get { return this.isRequired; }
        set { this.isRequired = value; }
    }

    Report IDetail<Report>.Master
    {
        get { return this.Report; }
    }

    public virtual int Order
    {
        get { return this.order; }
        set { this.order = value; }
    }

    public virtual string DisplayValueProperty
    {
        get { return this.displayValueProperty; }
        set { this.displayValueProperty = value; }
    }
}
