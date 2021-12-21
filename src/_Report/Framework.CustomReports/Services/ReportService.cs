using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Configuration.Core;
using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services.Persistent;
using Framework.CustomReports.Services.Runtime;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.CustomReports.Services
{
    public partial class ReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : IReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>

       where TBLLContext :
        IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,
        IAuthorizationBLLContextContainer<IAuthorizationBLLContext>,
        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode,Guid>>>,
        ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TSecurityOperationCode : struct, Enum
    {
        private readonly Dictionary<Guid, Func<TBLLContext, ICustomReportEvaluator>> evaluateReports;

        public ReportService(Dictionary<Guid, Func<TBLLContext, ICustomReportEvaluator>> evaluateReports)
        {
            this.evaluateReports = evaluateReports;
        }

        public IReportStream GetReportStream(ReportGenerationModel model, ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext)
        {
            return this.GetReportStreamService(model.Report.ReportType, reportServiceContext).GetReportByModel(model);
        }

        protected virtual IReportStreamService GetReportStreamService(ReportType reportType, ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext)
        {
            if (ReportType.Persistent == reportType)
            {
                return new PersistentReportStreamService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>(reportServiceContext.Context, reportServiceContext.SystemMetadataTypeBuilder);
            }

            return new CustomReportStreamService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>(this.evaluateReports, reportServiceContext.Context);
        }
    }
}
