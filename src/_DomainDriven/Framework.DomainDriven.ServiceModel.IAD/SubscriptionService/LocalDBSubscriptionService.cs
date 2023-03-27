using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Subscriptions;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD;

/// <summary>
/// Сервис для сохрания модификаций в локальную бд http://readthedocs/docs/iad-framework/en/master/KB/integrations/EventDTO.html
/// </summary>
public class LocalDBSubscriptionService : BLLContextContainer<IConfigurationBLLContext>, IStandardSubscriptionService
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="context">Контекст утилит</param>
    public LocalDBSubscriptionService(IConfigurationBLLContext context)
            : base(context)
    {
    }

    public void ProcessChanged([NotNull] ObjectModificationInfoDTO<Guid> changedObjectInfo)
    {
        if (changedObjectInfo == null) throw new ArgumentNullException(nameof(changedObjectInfo));

        var domainObjectModification = new DomainObjectModification
                                       {
                                               DomainType = this.Context.GetDomainType(changedObjectInfo.TypeInfoDescription),
                                               DomainObjectId = changedObjectInfo.Identity,
                                               Revision = changedObjectInfo.Revision,
                                               Type = changedObjectInfo.ModificationType
                                       };

        this.Context.Logics.Default.Create<DomainObjectModification>().Save(domainObjectModification);
    }
}
