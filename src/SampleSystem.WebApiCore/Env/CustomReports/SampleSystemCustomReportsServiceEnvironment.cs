using System;

using Framework.Core;

using SampleSystem.BLL;
using SampleSystem.CustomReports.BLL;
using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;
using Framework.CustomReports.Domain;
using Framework.CustomReports.WebApi;

using SampleSystem.CustomReports.Employee;

namespace SampleSystem.WebApiCore.CustomReports
{
    public class SampleSystemCustomReportsServiceEnvironment : ReportDefinitionServiceEnvironment<SampleSystemServiceEnvironment,
                                                               ISampleSystemBLLContext, PersistentDomainObjectBase, SampleSystemSecurityOperationCode>,
                                                               ISecurityOperationCodeProviderContainer<SampleSystemSecurityOperationCode>
    {
        private static SampleSystemServiceEnvironment env;

        private static readonly Lazy<SampleSystemCustomReportsServiceEnvironment> CurrentLazy = LazyHelper.Create(
            () => new SampleSystemCustomReportsServiceEnvironment(env));


        private readonly SecurityOperationCodeProvider securityOperationCodeProvider = new SecurityOperationCodeProvider();
        public SampleSystemCustomReportsServiceEnvironment(SampleSystemServiceEnvironment serviceEnvironment) : base(serviceEnvironment, new CustomReportAssembly().WithDomainAssembly(typeof(EmployeeReport).Assembly).WithBLLAssembly(typeof(EmployeeReportBLL).Assembly))
        {
            env = serviceEnvironment;
        }

        public static SampleSystemCustomReportsServiceEnvironment Current => CurrentLazy.Value;

        public ISecurityOperationCodeProvider<SampleSystemSecurityOperationCode> SecurityOperationCodeProvider => this.securityOperationCodeProvider;
    }
}
