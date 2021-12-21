using System.Collections.Generic;
using Framework.CustomReports.Domain;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.CustomReports.WebApi
{
    public interface ISyncCustomReportService<TBLLContext, TSecurityOperationCode>
        where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>
    {
        void Sync(IServiceEnvironment<TBLLContext> serviceEnvironment, IList<ICustomReport<TSecurityOperationCode>> runtimeReports);
    }
}
