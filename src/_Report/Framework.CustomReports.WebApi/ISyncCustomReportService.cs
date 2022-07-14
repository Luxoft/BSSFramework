using System.Collections.Generic;
using Framework.CustomReports.Domain;
using Framework.DomainDriven.BLL.Configuration;

namespace Framework.CustomReports.WebApi
{
    public interface ISyncCustomReportService<TBLLContext, TSecurityOperationCode>
        where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>
    {
        void Sync(IList<ICustomReport<TSecurityOperationCode>> runtimeReports);
    }
}
