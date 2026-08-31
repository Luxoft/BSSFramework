using Framework.Database.Audit;
using Framework.Database.EntityFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.Audit.DependencyInjection;

public class AuditEntityFrameworkSetupExtension : IEntityFrameworkSetupExtension
{
    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<IRevisionService, EfRevisionService>();
    }
}
