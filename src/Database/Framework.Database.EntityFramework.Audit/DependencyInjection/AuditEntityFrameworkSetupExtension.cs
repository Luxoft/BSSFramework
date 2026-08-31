using Anch.DependencyInjection;

using Framework.Database.Audit;
using Framework.Database.EntityFramework.DependencyInjection;
using Framework.Database.EntityFramework.Sessions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.Audit.DependencyInjection;

public class AuditEntityFrameworkSetupExtension : IEntityFrameworkSetupExtension
{
    public void AddServices(IServiceCollection services)
    {
        services.AddScopedFrom((DbContext dbContext) => ((IInfrastructure<IServiceProvider>)dbContext).Instance.GetRequiredService<EfCurrentRevisionState>())
                .AddScoped<IRevisionService, EfRevisionService>();
    }
}
