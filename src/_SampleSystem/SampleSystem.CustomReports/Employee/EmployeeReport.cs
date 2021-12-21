using System;

using Framework.CustomReports.Domain;

namespace SampleSystem.CustomReports.Employee
{
    public class EmployeeReport : SampleSystemCustomReportBase<EmployeeReportParameter>
    {
        public EmployeeReport()
        {

        }

        public override Guid Id => new Guid("7DBDCCDF-1F43-43FA-854A-D465F5F4ED53");

        public override IAccessReportRight AccessReportRight => AccessReportRights.ForAll();

        public override SampleSystemSecurityOperationCode SecurityOperation => SampleSystemSecurityOperationCode.EmployeeView;
    }
}
