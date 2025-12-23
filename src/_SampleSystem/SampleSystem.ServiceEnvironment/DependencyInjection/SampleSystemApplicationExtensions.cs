using CommonFramework.RelativePath.DependencyInjection;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.WebApi.Utils.SL;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemApplicationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterGeneralApplicationServices(IConfiguration configuration) =>
            services.AddHttpContextAccessor()
                    .AddLogging()
                    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EmployeeBLL>())
                    .RegisterSmtpNotification(configuration)
                    .AddRelativePaths()
                    .RegisterApplicationServices();

        private IServiceCollection AddRelativePaths() =>
            services.AddRelativeDomainPath((TestExceptObject v) => v.Employee)
                    .AddRelativeDomainPath((TestRelativeEmployeeObject v) => v.EmployeeRef1, nameof(TestRelativeEmployeeObject.EmployeeRef1))
                    .AddRelativeDomainPath((TestRelativeEmployeeObject v) => v.EmployeeRef2, nameof(TestRelativeEmployeeObject.EmployeeRef2))
                    .AddRelativeDomainPath((TestRelativeEmployeeParentObject v) => v.Children.Select(c => c.Employee));

        private IServiceCollection RegisterApplicationServices() =>
            services.AddScoped<ExampleFaultDALListenerSettings>()
                    .AddScoped<IExampleServiceForRepository, ExampleServiceForRepository>()
                    .AddScoped<SampleSystemCustomAribaLocalDBEventMessageSender>()
                    .AddSingleton<ISlJsonCompatibilitySerializer, SlJsonCompatibilitySerializer>() // For SL
                    .AddKeyedSingleton("DTO", TypeResolverHelper.Create(TypeSource.FromSample<BusinessUnitSimpleDTO>(), TypeSearchMode.Both)); // For legacy audit

        private IServiceCollection RegisterSmtpNotification(IConfiguration configuration)
        {
            services.RegisterNotificationJob();
            services.RegisterNotificationSmtp(configuration);
            services.RegisterRewriteReceiversDependencies(configuration);

            return services;
        }
    }
}
