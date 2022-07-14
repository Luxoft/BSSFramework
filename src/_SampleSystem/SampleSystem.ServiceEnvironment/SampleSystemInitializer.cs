using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.SubscriptionModeling;
using Framework.DomainDriven.BLL;

using SampleSystem.BLL;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemInitializer
{
    private readonly IContextEvaluator<ISampleSystemBLLContext> contextEvaluator;

    private readonly SubscriptionMetadataStore subscriptionMetadataStore;

    public SampleSystemInitializer(IContextEvaluator<ISampleSystemBLLContext> contextEvaluator, SubscriptionMetadataStore subscriptionMetadataStore)
    {
        this.contextEvaluator = contextEvaluator;
        this.subscriptionMetadataStore = subscriptionMetadataStore;
    }
    public void Initialize()
    {
        this.contextEvaluator.Evaluate(
                                       DBSessionMode.Write,
                                       context =>
                                       {
                                           context.Configuration.Logics.NamedLock.CheckInit();
                                       });

        this.contextEvaluator.Evaluate(
                                       DBSessionMode.Write,
                                       context =>
                                       {
                                           context.Logics.NamedLock.CheckInit();
                                       });

        this.contextEvaluator.Evaluate(
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

        this.contextEvaluator.Evaluate(
                                       DBSessionMode.Write,
                                       context =>
                                       {
                                           context.Configuration.Logics.SystemConstant.Initialize(typeof(SampleSystemSystemConstant));
                                       });

        this.contextEvaluator.Evaluate(
                                       DBSessionMode.Write,
                                       context => this.subscriptionMetadataStore.RegisterCodeFirstSubscriptions(context.Configuration.Logics.CodeFirstSubscription, context.Configuration));
    }
}
