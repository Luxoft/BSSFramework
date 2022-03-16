using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Cap;
using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.Attachments.BLL;
using Framework.Attachments.Environment;
using Framework.Attachments.Generated.DAL.NHibernate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Generated.DAL.NHibernate;
using AttachmentsSampleSystem.ServiceEnvironment;
using AttachmentsSampleSystem.WebApiCore.Env.Database;

using UserAuthenticationService = AttachmentsSampleSystem.WebApiCore.Env.UserAuthenticationService;

namespace AttachmentsSampleSystem.WebApiCore
{
    public static class EnvironmentExtensions
    {
        public static IServiceCollection AddEnvironment(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services
                .AddHttpContextAccessor();

            services.AddDatabaseSettings(connectionString);
            services.AddCapBss(connectionString);

            services.RegisterMessageSenderDependencies<AttachmentsSampleSystemServiceEnvironment, IAttachmentsSampleSystemBLLContext>(configuration);
            services.RegisterRewriteReceiversDependencies(configuration);

            // Others
            services.AddSingleton<IDateTimeService>(DateTimeService.Default);
            services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();
            services.AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>();


            return services.AddSingleton<AttachmentsSampleSystemServiceEnvironment>()
                           .AddControllerEnvironment();
        }

        public static IServiceCollection AddDatabaseSettings(this IServiceCollection services, string connectionString) =>
                services
                        .AddSingleton<IDBSessionFactory, AttachmentsSampleSystemNHibSessionFactory>()
                        .AddSingleton<NHibConnectionSettings>()
                        .AddSingleton<IMappingSettings>(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(AttachmentsMappingSettings.CreateWithoutAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(
                                                        new AttachmentsSampleSystemMappingSettings(
                                                                                        new DatabaseName(string.Empty, "app"),
                                                                                        connectionString));

        public static IServiceCollection AddControllerEnvironment(this IServiceCollection services)
        {
            services.AddSingleton<IExceptionProcessor, ApiControllerExceptionService<IServiceEnvironment<IAttachmentsSampleSystemBLLContext>, IAttachmentsSampleSystemBLLContext>>();

            // Environment
            services
                .AddSingleton<IServiceEnvironment<IAttachmentsSampleSystemBLLContext>>(x => x.GetRequiredService<AttachmentsSampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IAuthorizationBLLContext>>(x => x.GetRequiredService<AttachmentsSampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IConfigurationBLLContext>>(x => x.GetRequiredService<AttachmentsSampleSystemServiceEnvironment>())

                .AddSingleton<IServiceEnvironment<IAttachmentsBLLContext>>(x => x.GetRequiredService<AttachmentsSampleSystemServiceEnvironment>().AttachmentsModule);

            return services;
        }
    }
}
