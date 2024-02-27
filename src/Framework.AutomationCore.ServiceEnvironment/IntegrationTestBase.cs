using Automation.Enums;
using Automation.ServiceEnvironment.Services;
using Automation.Utils.DatabaseUtils;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.Notification.DTO;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public abstract class IntegrationTestBase<TBLLContext> : RootServiceProviderContainer<TBLLContext>
    where TBLLContext : IServiceProviderContainer
{
    private readonly IServiceProviderPool rootServiceProviderPool;

    protected IntegrationTestBase(IServiceProviderPool rootServiceProviderPool)
        : base(rootServiceProviderPool.Get())
    {
        this.rootServiceProviderPool = rootServiceProviderPool;
    }

    public virtual void Initialize()
    {
        this.ReattachDatabase();
        this.ClearNotifications();
        this.ClearIntegrationEvents();
        this.ResetServices();
    }

    public virtual void Cleanup()
    {
        try
        {
            this.DropDatabaseAfterTest();
            this.CleanupTestEnvironment();
        }
        finally
        {
            this.ReleaseServiceProvider();
        }
    }

    protected virtual void ResetServices()
    {
        this.RootServiceProvider.GetService<IIntegrationTestUserAuthenticationService>()?.Reset();
        this.RootServiceProvider.GetService<IntegrationTestTimeProvider>()?.Reset();
    }

    protected virtual void DropDatabaseAfterTest()
    {
        if (this.ConfigUtil.UseLocalDb || this.ConfigUtil.TestRunMode == TestRunMode.DefaultRunModeOnEmptyDatabase)
        {
            AssemblyInitializeAndCleanup.RunAction("Drop Database", this.DatabaseContext.Drop);
        }
    }

    protected virtual void ReleaseServiceProvider()
        => this.rootServiceProviderPool.Release(this.RootServiceProvider);

    protected virtual void ReattachDatabase()
    {
        switch (this.ConfigUtil.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
            case TestRunMode.RestoreDatabaseUsingAttach:
                AssemblyInitializeAndCleanup.RunAction("Drop Database", this.DatabaseContext.Drop);
                AssemblyInitializeAndCleanup.RunAction("Restore Databases", this.DatabaseContext.AttachDatabase);
                break;
        }
    }

    protected virtual void CleanupTestEnvironment()
    {
    }

    protected IConfigurationBLLContext GetConfigurationBLLContext(TBLLContext context)
    {
        return context.ServiceProvider.GetRequiredService<IConfigurationBLLContext>();
    }

    /// <summary>
    /// Отчистка списка нотифицаций
    /// </summary>
    public virtual void ClearNotifications()
    {
        this.EvaluateWrite(context => this.GetConfigurationBLLContext(context).Logics.DomainObjectNotification.Pipe(bll => bll.GetFullList().ForEach(bll.Remove)));
    }

    /// <summary>
    /// Получение списка модификаций
    /// </summary>
    /// <returns></returns>
    protected virtual List<ObjectModificationInfoDTO<Guid>> GetModifications()
    {
        return this.EvaluateRead(
            context =>

                this.GetConfigurationBLLContext(context).Logics.DomainObjectModification.GetFullList()
                    .ToList(
                        mod => new ObjectModificationInfoDTO<Guid>
                               {
                                   Identity = mod.DomainObjectId,
                                   ModificationType = mod.Type,
                                   Revision = mod.Revision,
                                   TypeInfoDescription = new TypeInfoDescriptionDTO(mod.DomainType)
                               }));
    }

    /// <summary>
    /// Отчистка списка модификаций
    /// </summary>
    protected virtual void ClearModifications()
    {
        this.EvaluateWrite(context => this.GetConfigurationBLLContext(context).Logics.DomainObjectModification.Pipe(bll => bll.GetFullList().ForEach(bll.Remove)));
    }

    /// <summary>
    /// Отчистка интеграционных евентов
    /// </summary>
    public virtual void ClearIntegrationEvents()
    {
        this.ClearModifications();

        this.EvaluateWrite(context =>
        {
            var bll = this.GetConfigurationBLLContext(context).Logics.Default.Create<Framework.Configuration.Domain.DomainObjectEvent>();

            bll.GetFullList().ForEach(bll.Remove);
        });
    }

    /// <summary>
    /// Получение интегационных евентов
    /// </summary>
    /// <returns></returns>
    protected virtual List<T> GetIntegrationEvents<T>(string queueTag = "default")
    {
        var serializeType = typeof(T).FullName;

        return this.EvaluateRead(
            context => this.GetConfigurationBLLContext(context).Logics.DomainObjectEvent
                .GetListBy(v => v.SerializeType == serializeType && v.QueueTag == queueTag)
                .ToList(obj => DataContractSerializerHelper.Deserialize<T>(obj.SerializeData)));
    }

    /// <summary>
    /// Получение списка нотификаций
    /// </summary>
    /// <returns></returns>
    protected virtual List<NotificationEventDTO> GetNotifications()
    {
        return this.EvaluateRead(
            context => this.GetConfigurationBLLContext(context).Logics.DomainObjectNotification.GetFullList()
                .ToList(obj => DataContractSerializerHelper.Deserialize<NotificationEventDTO>(obj.SerializeData)));
    }
}
