using Framework.Subscriptions.Domain;

namespace Framework.Configuration.BLL;

public interface IDomainObjectVersionsResolver
{
    DomainObjectVersions GetDomainObjectVersions(Guid domainObjectId, long revisionNumber);
}
