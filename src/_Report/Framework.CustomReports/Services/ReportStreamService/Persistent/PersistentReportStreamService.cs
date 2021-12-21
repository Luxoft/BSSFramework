using System;
using System.IO;
using System.Linq;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services.Persistent.Strategy;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.CustomReports.Services.Persistent
{
    public class PersistentReportStreamService<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : IReportStreamService where TMainBLLContext :
        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
        DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Configuration.BLL.IConfigurationBLLContext>,
        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,
        ISecurityServiceContainer<IRootSecurityService<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> where TPersistentDomainObjectBase : class, IIdentityObject<Guid> where TSecurityOperationCode : struct, Enum
    {
        private readonly TMainBLLContext _context;

        private readonly ISystemMetadataTypeBuilder _systemMetadataTypeBuilder;

        public PersistentReportStreamService(
            TMainBLLContext context,
            ISystemMetadataTypeBuilder systemMetadataTypeBuilder)
        {
            this._context = context;
            this._systemMetadataTypeBuilder = systemMetadataTypeBuilder;
        }


        public IReportStream GetReportByModel(ReportGenerationModel model)
        {
            var report = model.Report;

            var domainType = this._systemMetadataTypeBuilder.TypeResolver.Resolve(new TypeHeader(report.DomainTypeName));

            var func = new Func<ReportGenerationModel, Stream>(this.GetStream<TPersistentDomainObjectBase>);

            var genericMethod = func.Method.GetGenericMethodDefinition().MakeGenericMethod(domainType);

            var anonResult = genericMethod.Invoke(this, new object[] { model });

            var stream = (Stream)anonResult;

            return new ReportStream(report.Name, ReportStreamType.Excel, stream);
        }


        private Stream GetStream<TDomain>(ReportGenerationModel model)
            where TDomain : class, TPersistentDomainObjectBase
        {
            var reportPropertyToMetadataLinks = model.Report.GetReportPropertyToMetadataLinks<TDomain>(this._systemMetadataTypeBuilder.SystemMetadata).ToList();

            // load from db full domain if report with any virtual property or with any viewsecurityattributes property (ignore edit security attribute)
            if (model.Report.HasVirtualFilter<TDomain>(this._systemMetadataTypeBuilder.SystemMetadata) || reportPropertyToMetadataLinks.HasVirtualProperty() || reportPropertyToMetadataLinks.HasViewSecurityAttributes())
            {
                return new FullDomainReportStreamStrategy<TMainBLLContext, TDomain, TPersistentDomainObjectBase, TSecurityOperationCode>(this._context, this._systemMetadataTypeBuilder.TypeResolver, this._systemMetadataTypeBuilder.SystemMetadata, this._systemMetadataTypeBuilder.AnonymousTypeBuilder, reportPropertyToMetadataLinks)
                    .Generate(model, reportPropertyToMetadataLinks);
            }

            return new ProjectionReportStreamStrategy<TMainBLLContext, TDomain, TPersistentDomainObjectBase, TSecurityOperationCode>(this._context, this._systemMetadataTypeBuilder.SystemMetadata)
                .Generate(model, reportPropertyToMetadataLinks);
        }
    }
}
