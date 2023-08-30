using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain.Reports;

/// <summary>
/// Базовый класс для выдачи прав на отчет
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AccessableReportRightsBase<T> : AuditPersistentDomainObjectBase, IDetail<Report>
{
    private Report report;

    private T value;

    protected AccessableReportRightsBase(Report report)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));

        this.Report = report;
    }

    protected AccessableReportRightsBase()
    {
    }

    Report IDetail<Report>.Master
    {
        get { return this.Report; }
    }

    public virtual Report Report
    {
        get { return this.report; }
        private set { this.report = value; }
    }

    [Required]
    public virtual T Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
}
