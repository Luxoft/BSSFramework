using System;

using Framework.Persistent;

namespace Framework.Configuration.Domain.Reports;

/// <summary>
/// Выдача прав на отчет по бизнес роли
/// </summary>
public class AccessableBusinessRoleReportRight : AccessableReportRightsBase<Guid>
{
    protected AccessableBusinessRoleReportRight()
    {
        // For NHibernate
    }

    public AccessableBusinessRoleReportRight(Report report)
            : base(report)
    {
        this.Report.AddDetail(this);
    }
}
