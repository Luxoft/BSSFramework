using System;

using Framework.Persistent;

namespace Framework.Configuration.Domain.Reports;

/// <summary>
/// Выдача прав на отчет по операции
/// </summary>
public class AccessableOperationReportRight : AccessableReportRightsBase<Guid>
{
    protected AccessableOperationReportRight()
    {
        // For NHibernate
    }

    public AccessableOperationReportRight(Report report)
            : base(report)
    {
        this.Report.AddDetail(this);
    }
}
