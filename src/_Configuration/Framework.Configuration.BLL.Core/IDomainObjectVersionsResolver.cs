using Framework.Subscriptions.Domain;

namespace Framework.Configuration.BLL;

public interface IDomainObjectVersionsResolver
{
    IDomainObjectVersions GetDomainObjectVersions(Guid domainObjectId, long revisionNumber);
}
