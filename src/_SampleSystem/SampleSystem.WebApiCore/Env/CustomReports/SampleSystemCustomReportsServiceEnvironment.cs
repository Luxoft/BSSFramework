using SampleSystem.BLL;
using SampleSystem.CustomReports.BLL;
using SampleSystem.Domain;

using Framework.CustomReports.Domain;
using Framework.CustomReports.WebApi;
using Framework.DomainDriven;
using Framework.DomainDriven.SerializeMetadata;

using SampleSystem.CustomReports.Employee;

namespace SampleSystem.WebApiCore.CustomReports
{
    public class SampleSystemCustomReportsServiceEnvironment : ReportDefinitionServiceEnvironment<ISampleSystemBLLContext, PersistentDomainObjectBase, SampleSystemSecurityOperationCode>
    {
        public SampleSystemCustomReportsServiceEnvironment(
                ISystemMetadataTypeBuilder systemMetadataTypeBuilder,
                IContextEvaluator<ISampleSystemBLLContext> contextEvaluator) :
                base(systemMetadataTypeBuilder, contextEvaluator, new CustomReportAssembly().WithDomainAssembly(typeof(EmployeeReport).Assembly).WithBLLAssembly(typeof(EmployeeReportBLL).Assembly))
        {
        }
    }
}
