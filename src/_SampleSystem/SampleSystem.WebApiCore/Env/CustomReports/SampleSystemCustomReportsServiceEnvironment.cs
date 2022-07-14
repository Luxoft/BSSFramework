using System;

using Framework.Core;

using SampleSystem.BLL;
using SampleSystem.CustomReports.BLL;
using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;
using Framework.CustomReports.Domain;
using Framework.CustomReports.WebApi;
using Framework.DomainDriven.BLL;

using SampleSystem.CustomReports.Employee;

namespace SampleSystem.WebApiCore.CustomReports
{
    public class SampleSystemCustomReportsServiceEnvironment : ReportDefinitionServiceEnvironment<SampleSystemServiceEnvironment,
                                                               ISampleSystemBLLContext, PersistentDomainObjectBase, SampleSystemSecurityOperationCode>,
                                                               ISecurityOperationCodeProviderContainer<SampleSystemSecurityOperationCode>
    {
        private readonly SecurityOperationCodeProvider securityOperationCodeProvider = new SecurityOperationCodeProvider();

        public SampleSystemCustomReportsServiceEnvironment(SampleSystemServiceEnvironment serviceEnvironment, IContextEvaluator<ISampleSystemBLLContext> contextEvaluator)
                : base(serviceEnvironment, contextEvaluator, new CustomReportAssembly().WithDomainAssembly(typeof(EmployeeReport).Assembly).WithBLLAssembly(typeof(EmployeeReportBLL).Assembly))
        {
        }

        public ISecurityOperationCodeProvider<SampleSystemSecurityOperationCode> SecurityOperationCodeProvider => this.securityOperationCodeProvider;
    }
}
