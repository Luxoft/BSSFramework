using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Security.Cryptography;
using Framework.SecuritySystem.Rules.Builders;
using Framework.Validation;

using Microsoft.Extensions.Options;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

using AuditPersistentDomainObjectBase = SampleSystem.Domain.AuditPersistentDomainObjectBase;
using NamedLock = SampleSystem.Domain.NamedLock;
using NamedLockOperation = SampleSystem.Domain.NamedLockOperation;
using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;

namespace SampleSystem.ServiceEnvironment
{
    public abstract class SampleSystemServiceEnvironment :
        ServiceEnvironmentBase<SampleSystemBllContextContainer, ISampleSystemBLLContext, PersistentDomainObjectBase,
        AuditPersistentDomainObjectBase, SampleSystemSecurityOperationCode, NamedLock, NamedLockOperation>,
        ISystemMetadataTypeBuilderContainer
    {

        protected static readonly ITypeResolver<string> CurrentTargetSystemTypeResolver = new[] { TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver(), TypeSource.FromSample<BusinessUnitSimpleDTO>().ToDefaultTypeResolver() }.ToComposite();

        protected ICryptService<CryptSystem> CryptService { get; } = new CryptService<CryptSystem>();

        protected readonly bool? isDebugMode;
        protected readonly ValidatorCompileCache ValidatorCompileCache;
        protected readonly IFetchService<PersistentDomainObjectBase, FetchBuildRule> FetchService;
        protected readonly Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> SecurityExpressionBuilderFactoryFunc;

        protected readonly SmtpSettings smtpSettings;

        protected readonly IRewriteReceiversService rewriteReceiversService;


        protected SampleSystemServiceEnvironment(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            INotificationContext notificationContext,
            IUserAuthenticationService userAuthenticationService,
            IOptions<SmtpSettings> smtpSettings,
            IRewriteReceiversService rewriteReceiversService = null,
            bool? isDebugMode = null,
            Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<SampleSystem.Domain.PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc = null)
            : base(serviceProvider, sessionFactory, notificationContext, userAuthenticationService, new SampleSystemSubscriptionsMetadataFinder())
        {
            this.SystemMetadataTypeBuilder = new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.All, typeof(PersistentDomainObjectBase).Assembly);
            this.SecurityExpressionBuilderFactoryFunc = securityExpressionBuilderFactoryFunc;
            this.isDebugMode = isDebugMode;
            this.ValidatorCompileCache =

                sessionFactory
                    .AvailableValues
                    .ToValidation()
                    .ToBLLContextValidationExtendedData<ISampleSystemBLLContext, SampleSystem.Domain.PersistentDomainObjectBase, Guid>()
                    .Pipe(extendedData => new SampleSystemValidationMap(extendedData))
                    .ToCompileCache();
            this.FetchService = new SampleSystemMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<PersistentDomainObjectBase>.OData);

            this.smtpSettings = smtpSettings.Value;
            this.rewriteReceiversService = rewriteReceiversService;

            this.InitializeOperation(this.Initialize);
        }

        public ISystemMetadataTypeBuilder SystemMetadataTypeBuilder { get; }

        public override bool IsDebugMode => this.isDebugMode ?? base.IsDebugMode;



        protected override SampleSystemBllContextContainer CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            return new SampleSystemBllContextContainer(
                                                       this,
                                                       scopedServiceProvider,
                                                       this.DefaultAuthorizationValidatorCompileCache,
                                                       this.ValidatorCompileCache,
                                                       this.SecurityExpressionBuilderFactoryFunc,
                                                       this.FetchService,
                                                       this.CryptService,
                                                       CurrentTargetSystemTypeResolver,
                                                       session,
                                                       currentPrincipalName,
                                                       this.smtpSettings,
                                                       this.rewriteReceiversService);
        }


        public void Initialize()
        {
            var contextEvaluator = this.GetContextEvaluator();

            contextEvaluator.Evaluate(
                DBSessionMode.Write,
                context =>
                {
                    context.Configuration.Logics.NamedLock.CheckInit();
                });

            contextEvaluator.Evaluate(
                DBSessionMode.Write,
                context =>
                {
                    context.Logics.NamedLock.CheckInit();
                });

            contextEvaluator.Evaluate(
                DBSessionMode.Write,
                context =>
                {
                    context.Authorization.InitSecurityOperations();

                    context.Configuration.Logics.TargetSystem.RegisterBase();
                    context.Configuration.Logics.TargetSystem.Register<SampleSystem.Domain.PersistentDomainObjectBase>(true, true);

                    var extTypes = new Dictionary<Guid, Type>
                                   {
                                       { new Guid("{79AF1049-3EC0-46A7-A769-62A24AD4F74E}"), typeof(Framework.Configuration.Domain.Sequence) }
                                   };

                    context.Configuration.Logics.TargetSystem.Register<Framework.Configuration.Domain.PersistentDomainObjectBase>(false, true, extTypes: extTypes);
                    context.Configuration.Logics.TargetSystem.Register<Framework.Authorization.Domain.PersistentDomainObjectBase>(false, true);
                });

            contextEvaluator.Evaluate(
                DBSessionMode.Write,
                context =>
                {
                    context.Configuration.Logics.SystemConstant.Initialize(typeof(SampleSystemSystemConstant));
                });

            contextEvaluator.Evaluate(
                DBSessionMode.Write,
                context => this.SubscriptionMetadataStore.RegisterCodeFirstSubscriptions(context.Configuration.Logics.CodeFirstSubscription, context.Configuration));
        }
    }
}
