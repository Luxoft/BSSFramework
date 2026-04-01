using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.NHibernate.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNHibernate(this IServiceCollection services, Action<INHibernateSetup> setupAction) =>
        services.Initialize<NHibernateSetup>(setupAction);
}
