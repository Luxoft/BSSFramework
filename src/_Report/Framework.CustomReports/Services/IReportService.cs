using System;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

namespace Framework.CustomReports.Domain
{
    public interface IReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>
        where TBLLContext :
        IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,
        IAuthorizationBLLContextContainer<Authorization.BLL.IAuthorizationBLLContext>,
        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TSecurityOperationCode : struct, Enum

    {
        IReportStream GetReportStream(ReportGenerationModel model, ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext);
    }
}