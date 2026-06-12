using System.Linq.Expressions;

using Anch.Core;
using Anch.Core.ExpressionEvaluate;
using Anch.GenericQueryable;
using Anch.RelativePath;
using Anch.SecuritySystem;
using Anch.SecuritySystem.Providers;
using Anch.SecuritySystem.SecurityAccessor;
using Anch.SecuritySystem.Services;

using Framework.Authorization.Domain;

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

    public override async ValueTask<SecurityAccessorData> GetAccessorDataAsync(TDomainObject domainObject, CancellationToken ct) =>
        (await toBusinessRolePathInfo
               .GetRelativeObjects(domainObject)
               .ToAsyncEnumerable()
               .Select(async (br, ct) => await this.GetAccessorData(br, ct))
               .ToListAsync(ct))
        .Or();

    private async Task<SecurityAccessorData> GetAccessorData(BusinessRole businessRole, CancellationToken ct) =>
        SecurityAccessorData.Return(
            await availablePermissionSource
                  .GetQueryable(DomainSecurityRule.AnyRole with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() })
                  .Where(permission => permission.Role == businessRole)
                  .Select(permission => permission.Principal)
                  .Distinct()
                  .Select(principal => principal.Name)
                  .GenericToListAsync(ct));
}

