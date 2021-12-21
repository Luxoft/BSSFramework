using System;
using System.Collections.Generic;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.CustomReports.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

namespace Framework.CustomReports.Services.Runtime
{
    public class CustomReportStreamService<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : IReportStreamService where TMainBLLContext :
        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
        DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Configuration.BLL.IConfigurationBLLContext>,
        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,
        ISecurityServiceContainer<IRootSecurityService<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>

        where TPersistentDomainObjectBase : class, IIdentityObject<Guid> where TSecurityOperationCode : struct, Enum
    {

        private readonly TMainBLLContext _context;

        private readonly Dictionary<Guid, Func<TMainBLLContext, ICustomReportEvaluator>> evaluateReports;

        public CustomReportStreamService(Dictionary<Guid, Func<TMainBLLContext, ICustomReportEvaluator>> evaluateReports, TMainBLLContext context)
        {
            this._context = context;
            this.evaluateReports = evaluateReports;
        }


        public IReportStream GetReportByModel(ReportGenerationModel model)
        {
            return this.evaluateReports[model.Report.Id](this._context).GetReportStream(model);
        }
    }
}