using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public abstract class ByIdentsFilterBuilder<TDomainObject, TSecurityContext>(
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    ISecurityContextSource securityContextSource) : AccessorsFilterBuilder<TDomainObject>
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
        TDomainObject domainObject,
        HierarchicalExpandType expandType)
    {
        var securityObjects = this.GetSecurityObjects(domainObject).ToArray();

        var securityContextTypeId = securityContextSource.GetSecurityContextInfo(typeof(TSecurityContext)).Id;

        var fullAccessFilter = ExpressionHelper.Create(
            (IPermission permission) => !permission.GetRestrictions(typeof(TSecurityContext))
                                                   .Contains(securityContextTypeId));

        if (securityObjects.Any())
        {
            var securityIdents = hierarchicalObjectExpanderFactory
                                 .Create(typeof(TSecurityContext))
                                 .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

            return fullAccessFilter.BuildOr(
                permission =>

                    permission.GetRestrictions(typeof(TSecurityContext))
                              .Any(restrictionId => securityIdents.Contains(restrictionId)));
        }
        else
        {
            return fullAccessFilter;
        }
    }

    protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);
}
