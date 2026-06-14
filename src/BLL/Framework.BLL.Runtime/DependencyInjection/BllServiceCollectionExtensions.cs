using Anch.DependencyInjection;
using Anch.SecuritySystem;

using Framework.Application.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.BLL.DependencyInjection;

public static class BllServiceCollectionExtensions
{
    public static IServiceCollection AddBLL<TPersistentDomainObjectBase, TDomainObject, TIdent, TBllFactory, TBllFactoryImplement, TBll>(
        this IServiceCollection services)
        where TBllFactory : class, ISecurityBLLFactory<IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>, TDomainObject>,
        ISecurityBLLFactory<TBll>
        where TBllFactoryImplement : class, TBllFactory
        where TBll : class
        where TDomainObject : class, TPersistentDomainObjectBase
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent> =>
        services.AddScoped<TBllFactory, TBllFactoryImplement>()
                .AddScopedFrom<ISecurityBLLFactory<IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>, TDomainObject>,
                    TBllFactory>()
                .AddKeyedScoped<TBll>(
                    nameof(SecurityRule.Disabled),
                    (sp, _) => ((ISecurityBLLFactory<TBll>)sp.GetRequiredService<TBllFactory>()).Create(SecurityRule.Disabled))
                .AddKeyedScoped<TBll>(
                    nameof(SecurityRule.View),
                    (sp, _) => ((ISecurityBLLFactory<TBll>)sp.GetRequiredService<TBllFactory>()).Create(SecurityRule.View))
                .AddKeyedScoped<TBll>(
                    nameof(SecurityRule.Edit),
                    (sp, _) => ((ISecurityBLLFactory<TBll>)sp.GetRequiredService<TBllFactory>()).Create(SecurityRule.Edit));
}
