using System;

using Framework.Authorization.BLL;
using Framework.CustomReports.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

namespace Framework.CustomReports.Domain
{
    public interface IReportServiceContainer<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>
        where TBLLContext :
        DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Configuration.BLL.IConfigurationBLLContext>,
        IAuthorizationBLLContextContainer<IAuthorizationBLLContext>, IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>, IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>> where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TSecurityOperationCode : struct, Enum
    {
        IReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> ReportService { get; }

        IReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> ReportParameterValueService { get; }
    }
}