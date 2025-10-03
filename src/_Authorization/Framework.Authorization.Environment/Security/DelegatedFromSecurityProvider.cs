using CommonFramework.ExpressionEvaluate;
using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystemImpl;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.RelativeDomainPathInfo;

namespace Framework.Authorization.Environment.Security;

public class DelegatedFromSecurityProvider<TDomainObject>(
    IExpressionEvaluatorStorage expressionEvaluatorStorage,
    ICurrentPrincipalSource currentPrincipalSource,
    [FromKeyedServices(nameof(Permission.DelegatedFrom))] IRelativeDomainPathInfo<TDomainObject, Permission> toPermissionPathInfo)
    : CurrentPrincipalSecurityProvider<TDomainObject>(
        expressionEvaluatorStorage,
        currentPrincipalSource,
        toPermissionPathInfo.Select(permission => permission.Principal));
