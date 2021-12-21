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
    public abstract class ReportDefinitionServiceEnvironment<TServiceEnvironment, TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>

    : IReportServiceContainer<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>,
        IServiceEnvironment<TBLLContext>,
        ISystemMetadataTypeBuilderContainer

    where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>, IAuthorizationBLLContextContainer<IAuthorizationBLLContext>,
    IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
    ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
    IFetchServiceContainer<TPersistentDomainObjectBase, Framework.DomainDriven.FetchBuildRule>, IValidatorContainer, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> where TServiceEnvironment : class, IServiceEnvironment<TBLLContext>, ISystemMetadataTypeBuilderContainer
    where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    where TSecurityOperationCode : struct, Enum
    {
        private readonly TServiceEnvironment _serviceEnvironment;

        protected ReportDefinitionServiceEnvironment(TServiceEnvironment serviceEnvironment, CustomReportAssembly customReportAssembly)
        {
            this._serviceEnvironment = serviceEnvironment;

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


            this.GetSyncCustomReportService().Sync(this, customReports);


            this.ReportService = new ReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>(idToCreateCustomReportDict);

            this.ReportParameterValueService = new ReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>();

        }

        protected virtual ISyncCustomReportService<TBLLContext, TSecurityOperationCode> GetSyncCustomReportService()
        {
            return new SyncCustomReportService<TBLLContext, TSecurityOperationCode>(this.SystemMetadataTypeBuilder);
        }

        public TServiceEnvironment ServiceEnvironment => this._serviceEnvironment;

        public virtual IReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> ReportService
        {
            get;
            private set;
        }

        public virtual IReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> ReportParameterValueService
        { get; private set; }


        public IContextEvaluator<TBLLContext> GetContextEvaluator(IServiceProvider currentScopedServiceProvider = null) => this.ServiceEnvironment.GetContextEvaluator(currentScopedServiceProvider);

        public IBLLContextContainer<TBLLContext> GetBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            return this.ServiceEnvironment.GetBLLContextContainer(scopedServiceProvider, session, currentPrincipalName);
        }

        public IDBSessionFactory SessionFactory => this.ServiceEnvironment.SessionFactory;

        public bool IsDebugMode => this.ServiceEnvironment.IsDebugMode;

        public ISystemMetadataTypeBuilder SystemMetadataTypeBuilder => this.ServiceEnvironment.SystemMetadataTypeBuilder;

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


            var resultExpresssion = Expression.Lambda<Func<TBLLContext, ICustomReportEvaluator>>(newExpression, parameter);

            var resultFunc = resultExpresssion.Compile();

            return resultFunc;
        }
    }
}
