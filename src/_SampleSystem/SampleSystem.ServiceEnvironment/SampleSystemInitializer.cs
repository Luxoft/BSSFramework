using Framework.Authorization.SecuritySystem.Initialize;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.NamedLocks;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemInitializer(
    IServiceEvaluator<ISampleSystemBLLContext> contextEvaluator,
    SubscriptionMetadataStore subscriptionMetadataStore,
    IInitializeManager initializeManager)
{
    public async Task InitializeAsync(CancellationToken cancellationToken) =>
        await initializeManager.InitializeOperationAsync(async () => await this.InternalInitialize(cancellationToken));

    private async Task InternalInitialize(CancellationToken cancellationToken)
    {
        await contextEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async context => await context.ServiceProvider.GetRequiredService<INamedLockInitializer>().Initialize(cancellationToken));

        await this.InitSecurityAsync<IAuthorizationSecurityContextInitializer>(cancellationToken);
        await this.InitSecurityAsync<IAuthorizationBusinessRoleInitializer>(cancellationToken);

        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                context.Configuration.Logics.TargetSystem.RegisterBase();
                context.Configuration.Logics.TargetSystem.Register<SampleSystem.Domain.PersistentDomainObjectBase>(true, true);

                var extTypes = new Dictionary<Guid, Type>
                               {
                                   { new Guid("{79AF1049-3EC0-46A7-A769-62A24AD4F74E}"), typeof(Framework.Configuration.Domain.Sequence) }
                               };

                context.Configuration.Logics.TargetSystem.Register<Framework.Configuration.Domain.PersistentDomainObjectBase>(
                    false,
                    true,
                    extTypes: extTypes);
                context.Configuration.Logics.TargetSystem.Register<Framework.Authorization.Domain.PersistentDomainObjectBase>(false, true);
            });

        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            context => { context.Configuration.Logics.SystemConstant.Initialize(typeof(SampleSystemSystemConstant)); });

        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            context => subscriptionMetadataStore.RegisterCodeFirstSubscriptions(
                context.Configuration.Logics.CodeFirstSubscription,
                context.Configuration));
    }

    private async Task InitSecurityAsync<TSecurityInitializer>(CancellationToken cancellationToken)
        where TSecurityInitializer : ISecurityInitializer =>
        await contextEvaluator.Evaluate(
            DBSessionMode.Write,
            async context =>
                await context.ServiceProvider
                             .GetRequiredService<TSecurityInitializer>()
                             .Init(cancellationToken));
}
