using System;
using System.Collections.Generic;
using Automation.Enums;
using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Utils.DatabaseUtils;

using Framework.Core;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.Notification.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

namespace Automation.ServiceEnvironment;

public abstract class IntegrationTestBase<TBLLContext> : RootServiceProviderContainer<TBLLContext>
    where TBLLContext : IConfigurationBLLContextContainer<IConfigurationBLLContext>
{
    private readonly ServiceProviderPool rootServiceProviderPool;

    protected IntegrationTestBase(ServiceProviderPool rootServiceProviderPool)
        : base(rootServiceProviderPool.Get())
    {
        this.rootServiceProviderPool = rootServiceProviderPool;
    }

    public virtual void Initialize()
    {
        switch (this.ConfigUtil.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
            case TestRunMode.RestoreDatabaseUsingAttach:
                AssemblyInitializeAndCleanup.RunAction("Drop Database", this.DatabaseContext.Drop);
                AssemblyInitializeAndCleanup.RunAction("Restore Databases", this.DatabaseContext.AttachDatabase);
                break;
        }

        this.ClearNotifications();
        this.ClearIntegrationEvents();
    }

    public virtual void Cleanup()
    {
        if (this.ConfigUtil.UseLocalDb || this.ConfigUtil.TestRunMode == TestRunMode.DefaultRunModeOnEmptyDatabase)
        {
            AssemblyInitializeAndCleanup.RunAction("Drop Database", this.DatabaseContext.Drop);
        }

        this.CleanupTestEnvironment();
        this.rootServiceProviderPool.Release(this.RootServiceProvider);
    }

    public virtual void CleanupTestEnvironment()
    {
    }

    /// <summary>
    /// Отчистка списка нотифицаций
    /// </summary>
    public virtual void ClearNotifications()
    {
        this.EvaluateWrite(context => context.Configuration.Logics.DomainObjectNotification.Pipe(bll => bll.GetFullList().ForEach(bll.Remove)));
    }

    /// <summary>
    /// Получение списка модификаций
    /// </summary>
    /// <returns></returns>
    protected virtual List<ObjectModificationInfoDTO<Guid>> GetModifications()
    {
        return this.EvaluateRead(context =>

            context.Configuration.Logics.DomainObjectModification.GetFullList()
                .ToList(mod => new ObjectModificationInfoDTO<Guid> { Identity = mod.DomainObjectId, ModificationType = mod.Type, Revision = mod.Revision, TypeInfoDescription = new TypeInfoDescriptionDTO(mod.DomainType) }));
    }

    /// <summary>
    /// Отчистка списка модификаций
    /// </summary>
    protected virtual void ClearModifications()
    {
        this.EvaluateWrite(context => context.Configuration.Logics.DomainObjectModification.Pipe(bll => bll.GetFullList().ForEach(bll.Remove)));
    }

    /// <summary>
    /// Отчистка интеграционных евентов
    /// </summary>
    public virtual void ClearIntegrationEvents()
    {
        this.ClearModifications();

        this.EvaluateWrite(context =>
        {
            var bll = context.Configuration.Logics.Default.Create<Framework.Configuration.Domain.DomainObjectEvent>();

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
            context => context.Configuration.Logics.DomainObjectEvent
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
            context => context.Configuration.Logics.DomainObjectNotification.GetFullList()
                .ToList(obj => DataContractSerializerHelper.Deserialize<NotificationEventDTO>(obj.SerializeData)));
    }
}
