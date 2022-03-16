using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Security.Cryptography;
using Framework.SecuritySystem.Rules.Builders;
using Framework.Validation;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Generated.DTO;

using Framework.Attachments.BLL;
using Framework.DomainDriven.ServiceModel.Service;

using AuditPersistentDomainObjectBase = AttachmentsSampleSystem.Domain.AuditPersistentDomainObjectBase;
using NamedLock = AttachmentsSampleSystem.Domain.NamedLock;
using NamedLockOperation = AttachmentsSampleSystem.Domain.NamedLockOperation;
using PersistentDomainObjectBase = AttachmentsSampleSystem.Domain.PersistentDomainObjectBase;

namespace AttachmentsSampleSystem.ServiceEnvironment
{
    public class AttachmentsSampleSystemServiceEnvironment :
        ServiceEnvironmentBase
        <AttachmentsSampleSystemBLLContextContainer,
        IAttachmentsSampleSystemBLLContext, PersistentDomainObjectBase,
        AuditPersistentDomainObjectBase, AttachmentsSampleSystemSecurityOperationCode, NamedLock, NamedLockOperation>
    {
        protected static readonly ITypeResolver<string> CurrentTargetSystemTypeResolver = new[] { TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver(), TypeSource.FromSample<BusinessUnitSimpleDTO>().ToDefaultTypeResolver() }.ToComposite();

        protected ICryptService<CryptSystem> CryptService { get; } = new CryptService<CryptSystem>();

        protected readonly bool? isDebugMode;
        protected readonly ValidatorCompileCache ValidatorCompileCache;
        protected readonly IFetchService<PersistentDomainObjectBase, FetchBuildRule> FetchService;
        protected readonly Func<IAttachmentsSampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> SecurityExpressionBuilderFactoryFunc;



        public AttachmentsSampleSystemServiceEnvironment(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            INotificationContext notificationContext,
            IUserAuthenticationService userAuthenticationService,
            bool? isDebugMode = null,
            Func<IAttachmentsSampleSystemBLLContext, ISecurityExpressionBuilderFactory<AttachmentsSampleSystem.Domain.PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc = null)
            : base(serviceProvider, sessionFactory, notificationContext, userAuthenticationService)
        {
            this.SystemMetadataTypeBuilder = new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.All, typeof(PersistentDomainObjectBase).Assembly);
            this.SecurityExpressionBuilderFactoryFunc = securityExpressionBuilderFactoryFunc;
            this.isDebugMode = isDebugMode;
            this.ValidatorCompileCache =

                sessionFactory
                    .AvailableValues
                    .ToValidation()
                    .ToBLLContextValidationExtendedData<IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Domain.PersistentDomainObjectBase, Guid>()
                    .Pipe(extendedData => new AttachmentsSampleSystemValidationMap(extendedData))
                    .ToCompileCache();
            this.FetchService = new AttachmentsSampleSystemMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<PersistentDomainObjectBase>.OData);

            this.AttachmentsModule = new AttachmentsSampleSystemServiceEnvironmentModule(this);

            this.InitializeOperation(this.Initialize);
        }

        public ISystemMetadataTypeBuilder SystemMetadataTypeBuilder { get; }

        public override bool IsDebugMode => this.isDebugMode ?? base.IsDebugMode;

        public AttachmentsSampleSystemServiceEnvironmentModule AttachmentsModule { get; }

        protected override IEnumerable<IServiceEnvironmentModule<AttachmentsSampleSystemBLLContextContainer>> GetModules()
        {
            foreach (var baseModule in base.GetModules())
            {
                yield return baseModule;
            }

            yield return this.AttachmentsModule;
        }

        protected override AttachmentsSampleSystemBLLContextContainer CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            return new AttachmentsSampleSystemBLLContextContainer(
                                                       this,
                                                       scopedServiceProvider,
                                                       this.ValidatorCompileCache,
                                                       this.SecurityExpressionBuilderFactoryFunc,
                                                       this.FetchService,
                                                       this.CryptService,
                                                       CurrentTargetSystemTypeResolver,
                                                       session,
                                                       currentPrincipalName);
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
                (IAttachmentsSampleSystemBLLContext context) =>
                {
                    context.Authorization.InitSecurityOperations();

                    context.Configuration.Logics.TargetSystem.RegisterBase();
                    context.Configuration.Logics.TargetSystem.Register<AttachmentsSampleSystem.Domain.PersistentDomainObjectBase>(true, true);

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
                    context.Configuration.Logics.SystemConstant.Initialize(typeof(AttachmentsSampleSystemSystemConstant));
                });

            contextEvaluator.Evaluate(
                DBSessionMode.Write,
                context => this.SubscriptionMetadataStore.RegisterCodeFirstSubscriptions(context.Configuration.Logics.CodeFirstSubscription, context.Configuration));
        }
    }
}
