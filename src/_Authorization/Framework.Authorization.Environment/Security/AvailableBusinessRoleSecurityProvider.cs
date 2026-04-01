using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;
using CommonFramework.RelativePath;

using Framework.Authorization.Domain;

using GenericQueryable;

using SecuritySystem;
using SecuritySystem.Providers;
using SecuritySystem.SecurityAccessor;
using SecuritySystem.Services;

namespace Framework.Authorization.Environment.Security;

public class AvailableBusinessRoleSecurityProvider<TDomainObject>(
    IExpressionEvaluatorStorage expressionEvaluatorStorage,
    IAvailablePermissionSource<Permission> availablePermissionSource,
    IRelativeDomainPathInfo<TDomainObject, BusinessRole> toBusinessRolePathInfo)
    : SecurityProvider<TDomainObject>(expressionEvaluatorStorage)
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =

        availablePermissionSource.GetQueryable(DomainSecurityRule.AnyRole)
                                 .Pipe(permissionQ =>
                                           ExpressionHelper.Create((BusinessRole businessRole) => permissionQ.Select(p => p.Role).Contains(businessRole)))
                                 .Pipe(toBusinessRolePathInfo.CreateCondition);

    public override async ValueTask<SecurityAccessorData> GetAccessorDataAsync(TDomainObject domainObject, CancellationToken cancellationToken) =>
        (await toBusinessRolePathInfo
               .GetRelativeObjects(domainObject)
               .ToAsyncEnumerable()
               .Select(async (br, ct) => await this.GetAccessorData(br, ct))
               .ToListAsync(cancellationToken))
        .Or();

    private async Task<SecurityAccessorData> GetAccessorData(BusinessRole businessRole, CancellationToken cancellationToken) =>
        SecurityAccessorData.Return(
            await availablePermissionSource
                  .GetQueryable(DomainSecurityRule.AnyRole with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() })
                  .Where(permission => permission.Role == businessRole)
                  .Select(permission => permission.Principal)
                  .Distinct()
                  .Select(principal => principal.Name)
                  .GenericToListAsync(cancellationToken));
}
