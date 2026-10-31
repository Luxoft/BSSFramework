using Framework.BLL;
using Framework.BLL.DTOMapping.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;

namespace Framework.Infrastructure.SubscriptionService;

/// <summary>
/// Service for saving modifications to the local db
/// </summary>
public class LocalDbObjectModificationProcessor : BLLContextContainer<IConfigurationBLLContext>, IObjectModificationProcessor
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context">Utilities context</param>
    public LocalDbObjectModificationProcessor(IConfigurationBLLContext context)
        : base(context)
    {
    }

    public async Task ProcessChanged(ObjectModificationInfoDTO<Guid> changedObjectInfo, CancellationToken ct)
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
