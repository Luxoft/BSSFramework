using Anch.Core;

using Framework.Application;
using Framework.Application.Repository;
using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.BLL.DTOMapping.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Helpers;
using Framework.Database;
using Framework.Notification.DTO;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem;

namespace Framework.AutomationCore;

public abstract class IntegrationTestBase<TBLLContext>(IServiceProvider rootServiceProvider) : RootServiceProviderContainer<TBLLContext>(rootServiceProvider)
    where TBLLContext : IServiceProviderContainer
{
    public Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        UserCredential? customUserCredential,
        Func<TBLLContext, Task<TResult>> getResult) =>
        rootServiceProvider.GetRequiredService<IServiceEvaluator<TBLLContext>>().EvaluateAsync(sessionMode, customUserCredential, getResult);


    protected IConfigurationBLLContext GetConfigurationBLLContext(TBLLContext context) => context.ServiceProvider.GetRequiredService<IConfigurationBLLContext>();

    /// <summary>
    /// Отчистка списка нотифицаций
    /// </summary>
    public virtual void ClearNotifications() =>
        this.EvaluateWrite(context => this.GetConfigurationBLLContext(context).Logics.DomainObjectNotification.Pipe(bll => bll.GetFullList().ForEach(bll.Remove)));

    /// <summary>
    /// Получение списка модификаций
    /// </summary>
    /// <returns></returns>
    protected virtual List<ObjectModificationInfoDTO<Guid>> GetModifications() =>
        this.EvaluateRead(context =>

                              this.GetConfigurationBLLContext(context).Logics.DomainObjectModification.GetFullList()
                                  .ToList(mod => new ObjectModificationInfoDTO<Guid>
                                  {
                                      Identity = mod.DomainObjectId,
                                      ModificationType = mod.Type,
                                      Revision = mod.Revision,
                                      TypeInfoDescription = new TypeInfoDescriptionDTO { Name = mod.DomainType.Name, Namespace = mod.DomainType.Namespace }
                                  }));

    /// <summary>
    /// Отчистка списка модификаций
    /// </summary>
    protected virtual void ClearModifications() =>
        this.EvaluateWrite(context => this.GetConfigurationBLLContext(context).Logics.DomainObjectModification.Pipe(bll => bll.GetFullList().ForEach(bll.Remove)));

    /// <summary>
    /// Отчистка интеграционных евентов
    /// </summary>
    public virtual void ClearIntegrationEvents()
    {
        this.ClearModifications();

        this.EvaluateWrite(context =>
        {
            var bll = this.GetConfigurationBLLContext(context).Logics.Default.Create<DomainObjectEvent>();

            bll.GetFullList().ForEach(bll.Remove);
        });
    }

    protected int GetIntegrationEventCount() =>
        this.EvaluateRead(ctx => ctx.ServiceProvider.GetRequiredService<IRepositoryFactory<DomainObjectEvent>>().Create().GetQueryable().Count());


    /// <summary>
    /// Получение интегационных евентов
    /// </summary>
    /// <returns></returns>
    protected virtual List<T> GetIntegrationEvents<T>(string queueTag = "default")
    {
        var serializeType = typeof(T).FullName;

        return this.EvaluateRead(context => this.GetConfigurationBLLContext(context).Logics.DomainObjectEvent
                                                .GetListBy(v => v.SerializeType == serializeType && v.QueueTag == queueTag)
                                                .ToList(obj => DataContractSerializerHelper.Deserialize<T>(obj.SerializeData)));
    }

    /// <summary>
    /// Получение списка нотификаций
    /// </summary>
    /// <returns></returns>
    protected virtual List<NotificationEventDTO> GetNotifications() =>
        this.EvaluateRead(context => this.GetConfigurationBLLContext(context).Logics.DomainObjectNotification.GetFullList()
                                         .ToList(obj => DataContractSerializerHelper.Deserialize<NotificationEventDTO>(obj.SerializeData)));
}
