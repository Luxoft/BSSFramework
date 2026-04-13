using Framework.BLL;
using Framework.BLL.DTOMapping.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;

namespace Framework.Infrastructure.SubscriptionService;

/// <summary>
/// Сервис для сохрания модификаций в локальную бд
/// </summary>
public class LocalDbObjectModificationProcessor : BLLContextContainer<IConfigurationBLLContext>, IObjectModificationProcessor
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="context">Контекст утилит</param>
    public LocalDbObjectModificationProcessor(IConfigurationBLLContext context)
        : base(context)
    {
    }

    public async Task ProcessChanged(ObjectModificationInfoDTO<Guid> changedObjectInfo, CancellationToken cancellationToken)
    {
        var domainObjectModification = new DomainObjectModification
                                       {
                                           DomainType = this.Context.GetDomainType(changedObjectInfo.TypeInfoDescription.ToDomainObject()),
                                           DomainObjectId = changedObjectInfo.Identity,
                                           Revision = changedObjectInfo.Revision,
                                           Type = changedObjectInfo.ModificationType
                                       };

        this.Context.Logics.Default.Create<DomainObjectModification>().Save(domainObjectModification);
    }
}
