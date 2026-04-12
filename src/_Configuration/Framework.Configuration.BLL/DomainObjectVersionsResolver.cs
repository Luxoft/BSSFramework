using Framework.Application.Domain;
using Framework.BLL;
using Framework.Subscriptions.Domain;

using Microsoft.Extensions.Logging;

namespace Framework.Configuration.BLL;

public class DomainObjectVersionsResolver<TBLLContext, TDomainObject>(
    TBLLContext context,
    ILogger<DomainObjectVersionsResolver<TBLLContext, TDomainObject>> logger) : IDomainObjectVersionsResolver

    where TBLLContext : IDefaultBLLContext<TDomainObject, Guid>
    where TDomainObject : class, IIdentityObject<Guid>
{
    private readonly IRevisionBLL<TDomainObject, Guid> revisionBll = context.Logics.Default.Create<TDomainObject>();

    public DomainObjectVersions GetDomainObjectVersions(Guid domainObjectId, long revisionNumber)
    {
        var prev = this.GetPreviousDomainObjectByRevisionNumber(domainObjectId, revisionNumber);
        var next = this.GetDomainObjectByRevisionNumber(domainObjectId, revisionNumber);

        return new DomainObjectVersions<TDomainObject>(prev, next);
    }

    private TDomainObject? GetPreviousDomainObjectByRevisionNumber(Guid domainObjectId, long revisionNumber)
    {
        var previousRevisionNumber = this.revisionBll.GetPreviousRevision(domainObjectId, revisionNumber);

        if (previousRevisionNumber == null)
        {
            return null;
        }
        else
        {
            var result = this.GetDomainObjectByRevisionNumber(domainObjectId, previousRevisionNumber.Value);

            logger.LogDebug(
                "Previous domain object revision '{result}' has been found by domain object id '{domainObjectId}' and revision number '{revisionNumber}'.",
                result,
                domainObjectId,
                revisionNumber);

            return result;
        }
    }

    private TDomainObject GetDomainObjectByRevisionNumber(Guid domainObjectId, long revisionNumber)
    {
        logger.LogDebug("Get current domain object revision by domain object id '{domainObjectId}' and revision number '{revisionNumber}'.", domainObjectId, revisionNumber);

        var result = this.revisionBll.GetObjectByRevision(domainObjectId, revisionNumber);

        logger.LogDebug(
            "Current domain object revision '{result}' has been found by domain object id '{domainObjectId}' and revision number '{revisionNumber}'.",
            result,
            domainObjectId,
            revisionNumber);

        return result;
    }
}
