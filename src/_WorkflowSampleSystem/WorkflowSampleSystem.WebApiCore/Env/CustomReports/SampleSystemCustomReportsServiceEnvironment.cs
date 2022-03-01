using System;

using Framework.Core;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.CustomReports.BLL;
using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.ServiceEnvironment;
using Framework.CustomReports.Domain;
using Framework.CustomReports.WebApi;

using WorkflowSampleSystem.CustomReports.Employee;

namespace WorkflowSampleSystem.WebApiCore.CustomReports
{
    public class WorkflowSampleSystemCustomReportsServiceEnvironment : ReportDefinitionServiceEnvironment<WorkflowSampleSystemServiceEnvironment,
                                                               IWorkflowSampleSystemBLLContext, PersistentDomainObjectBase, WorkflowSampleSystemSecurityOperationCode>,
                                                               ISecurityOperationCodeProviderContainer<WorkflowSampleSystemSecurityOperationCode>
    {
        private static WorkflowSampleSystemServiceEnvironment env;

        private static readonly Lazy<WorkflowSampleSystemCustomReportsServiceEnvironment> CurrentLazy = LazyHelper.Create(
            () => new WorkflowSampleSystemCustomReportsServiceEnvironment(env));


        private readonly SecurityOperationCodeProvider securityOperationCodeProvider = new SecurityOperationCodeProvider();
        public WorkflowSampleSystemCustomReportsServiceEnvironment(WorkflowSampleSystemServiceEnvironment serviceEnvironment) : base(serviceEnvironment, new CustomReportAssembly().WithDomainAssembly(typeof(EmployeeReport).Assembly).WithBLLAssembly(typeof(EmployeeReportBLL).Assembly))
        {
            env = serviceEnvironment;
        }

        public static WorkflowSampleSystemCustomReportsServiceEnvironment Current => CurrentLazy.Value;

        public ISecurityOperationCodeProvider<WorkflowSampleSystemSecurityOperationCode> SecurityOperationCodeProvider => this.securityOperationCodeProvider;
    }
}
