using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.Validation;

namespace Framework.CustomReports.WebApi
{
    public abstract class ReportDefinitionServiceEnvironment<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>

        where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>, IAuthorizationBLLContextContainer<IAuthorizationBLLContext>,
            IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
            ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
            IFetchServiceContainer<TPersistentDomainObjectBase, Framework.DomainDriven.FetchBuildRule>, IValidatorContainer, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TSecurityOperationCode : struct, Enum
    {
        private readonly ISystemMetadataTypeBuilder systemMetadataTypeBuilder;

        private readonly IContextEvaluator<TBLLContext> contextEvaluator;

        protected ReportDefinitionServiceEnvironment(ISystemMetadataTypeBuilder systemMetadataTypeBuilder, IContextEvaluator<TBLLContext> contextEvaluator, CustomReportAssembly customReportAssembly)
        {
            this.systemMetadataTypeBuilder = systemMetadataTypeBuilder;
            this.contextEvaluator = contextEvaluator;

            var links = new CustomReportParameterLinkService(customReportAssembly).GetLinks();

            var customReports = links
                .Select(z => z.CustomReportType)
                .Select(Activator.CreateInstance)
                .Cast<ICustomReport<TSecurityOperationCode>>()
                .ToList();

            var customReportBLLTypes = customReportAssembly
                .CustomReportBllTypes
                .Select(z => new { CustomReportBLLType = z, ParameterType = z.GetGenericArguments(typeof(ICustomReportBLL<>))[0] })
                .ToList();

            var missingReportParameters = customReports.Select(z => z.ParameterType).Except(customReportBLLTypes.Select(z => z.ParameterType)).ToList();
            if (missingReportParameters.Any())
            {
                throw new ArgumentException($"Absent custom report bll for parameters:'{string.Join(",", missingReportParameters.Select(z => z.FullName))}'");
            }

            var parameterTypeToCustomReportBLL = customReportBLLTypes.ToDictionary(z => z.ParameterType, z => z.CustomReportBLLType);

            var idGrouped = customReports.GroupBy(z => z.Id, (key, source) => new { Id = key, Reports = source.ToList() }).ToList();

            var failed = idGrouped.Where(z => z.Reports.Count > 1).Select(z => $"Reports'{z.Reports.Select(q => q.Name).Join(",")}' has same key:'{z.Id}'").Join(Environment.NewLine);
            if (!string.IsNullOrWhiteSpace(failed))
            {
                throw new ArgumentException(failed);
            }

            var idToCreateCustomReportDict = idGrouped.ToDictionary(z => z.Id,
                z => this.GetCreateCustomReportBLL(parameterTypeToCustomReportBLL[z.Reports.First().ParameterType]));


            this.GetSyncCustomReportService().Sync(customReports);


            this.ReportService = new ReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>(idToCreateCustomReportDict);

        }

        protected virtual ISyncCustomReportService<TBLLContext, TSecurityOperationCode> GetSyncCustomReportService()
        {
            return new SyncCustomReportService<TBLLContext, TSecurityOperationCode>(this.systemMetadataTypeBuilder, this.contextEvaluator);
        }

        public virtual IReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> ReportService { get; }

        protected virtual Func<TBLLContext, ICustomReportEvaluator> GetCreateCustomReportBLL(Type customReportType)
        {
            var targetConstructor = customReportType
                .GetConstructors()
                .FirstOrDefault(z => z.GetParameters().Length == 1 && z.GetParameters().First().ParameterType == typeof(TBLLContext));

            if (null == targetConstructor)
            {
                throw new ArgumentException($"Custom report system can't supported custom reports bll without constructor with one bllContext parameters. Failed custom report bll is: '{nameof(customReportType)}'");
            }
            var parameter = Expression.Parameter(typeof(TBLLContext));
            var newExpression = Expression.New(targetConstructor, parameter);


            var resultExpression = Expression.Lambda<Func<TBLLContext, ICustomReportEvaluator>>(newExpression, parameter);

            var resultFunc = resultExpression.Compile();

            return resultFunc;
        }
    }
}
