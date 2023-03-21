using Framework.Persistent;

namespace Framework.Configuration.Domain.Reports;

/// <summary>
/// Выдача прав на отчет конкретным логинам
/// </summary>
public class AccessablePrincipalReportRight : AccessableReportRightsBase<string>
{
    protected AccessablePrincipalReportRight()
    {
        // For NHibernate
    }

    public AccessablePrincipalReportRight(Report report)
            : base(report)
    {
        this.Report.AddDetail(this);
    }
}
