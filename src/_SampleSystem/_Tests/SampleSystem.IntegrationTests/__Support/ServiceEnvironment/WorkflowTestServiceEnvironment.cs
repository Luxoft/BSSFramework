using System;
using System.Collections.Generic;
using System.IO;

using Framework.Authorization.ApproveWorkflow;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Notification.DTO;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Security.Cryptography;
using Framework.SecuritySystem.Rules.Builders;
using Framework.Validation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;

using WorkflowCore.Interface;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class WorkflowTestServiceEnvironment : TestServiceEnvironment
{
    private static readonly Lazy<TestServiceEnvironment> DefaultLazy = new(CreateDefaultWorkflowTestServiceEnvironment);

    public WorkflowTestServiceEnvironment(IServiceProvider serviceProvider, IDBSessionFactory sessionFactory, EnvironmentSettings settings, IUserAuthenticationService userAuthenticationService, bool? isDebugMode = null)
            : base(serviceProvider, sessionFactory, settings, userAuthenticationService, isDebugMode)
    {
    }

    private static TestServiceEnvironment CreateDefaultWorkflowTestServiceEnvironment()
    {
        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", false, true)
                            .AddEnvironmentVariables(nameof(SampleSystem) + "_")
                            .Build();

        var serviceProvider = BuildServiceProvider(services => services.AddSingleton<SampleSystemServiceEnvironment>(sp => sp.GetRequiredService<WorkflowTestServiceEnvironment>())
                                                                       .AddSingleton<WorkflowTestServiceEnvironment>()
                                                                       .AddWorkflowCore(configuration)
                                                                       .AddAuthWorkflow());

        serviceProvider.StartWorkflow();

        return serviceProvider.GetRequiredService<WorkflowTestServiceEnvironment>();
    }



    protected override SampleSystemBllContextContainer CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
    {
        return new WorkflowTestSampleSystemBllContextContainer(
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

    private class WorkflowTestSampleSystemBllContextContainer : TestSampleSystemBllContextContainer
    {
        public WorkflowTestSampleSystemBllContextContainer(SampleSystemServiceEnvironment serviceEnvironment, IServiceProvider scopedServiceProvider, ValidatorCompileCache defaultAuthorizationValidatorCompileCache, ValidatorCompileCache validatorCompileCache, Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc, IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService, ICryptService<CryptSystem> cryptService, ITypeResolver<string> currentTargetSystemTypeResolver, IDBSession session, string currentPrincipalName, SmtpSettings smtpSettings, IRewriteReceiversService rewriteReceiversService)
                : base(serviceEnvironment, scopedServiceProvider, defaultAuthorizationValidatorCompileCache, validatorCompileCache, securityExpressionBuilderFactoryFunc, fetchService, cryptService, currentTargetSystemTypeResolver, session, currentPrincipalName, smtpSettings, rewriteReceiversService)
        {
        }

        protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender()
        {
            return new LocalDBNotificationEventDTOMessageSender(this.Configuration);
        }

        //protected override IEnumerable<IDALListener> GetDALFlushedListeners()
        //{
        //    foreach (var dalListener in base.GetDALFlushedListeners())
        //    {
        //        yield return dalListener;
        //    }

        //    yield return new PermissionWorkflowDALListener(this.Authorization, this.ServiceEnvironment.RootServiceProvider.GetRequiredService<IWorkflowHost>(), new WorkflowApproveProcessor(this.Authorization));
        //}

        protected override IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
        {
            foreach (var dalListener in base.GetBeforeTransactionCompletedListeners())
            {
                yield return dalListener;
            }

            yield return new PermissionWorkflowDALListener(this.Authorization, this.ServiceEnvironment.RootServiceProvider.GetRequiredService<IWorkflowHost>(), new WorkflowApproveProcessor(this.Authorization));
        }
    }


    public static TestServiceEnvironment Default => DefaultLazy.Value;
}
