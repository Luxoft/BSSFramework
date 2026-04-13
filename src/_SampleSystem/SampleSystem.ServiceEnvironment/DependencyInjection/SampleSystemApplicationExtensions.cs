using CommonFramework.RelativePath.DependencyInjection;

using Framework.Core.TypeResolving;
using Framework.Infrastructure.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public static class SampleSystemApplicationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddGeneralApplicationServices(IConfiguration configuration, IHostEnvironment hostEnvironment) =>

            services.AddHttpContextAccessor()
                    .AddLogging()
                    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EmployeeBLL>())
                    .AddApplicationNotification(configuration, hostEnvironment)
                    .AddRelativePaths()
                    .AddApplicationServices();

        private IServiceCollection AddRelativePaths() =>
            services.AddRelativeDomainPath((TestExceptObject v) => v.Employee)
                    .AddRelativeDomainPath((TestRelativeEmployeeObject v) => v.EmployeeRef1, nameof(TestRelativeEmployeeObject.EmployeeRef1))
                    .AddRelativeDomainPath((TestRelativeEmployeeObject v) => v.EmployeeRef2, nameof(TestRelativeEmployeeObject.EmployeeRef2))
                    .AddRelativeDomainPath((TestRelativeEmployeeParentObject v) => v.Children.Select(c => c.Employee));

        private IServiceCollection AddApplicationServices() =>
            services.AddScoped<ExampleFaultDALListenerSettings>()
                    .AddScoped<IExampleServiceForRepository, ExampleServiceForRepository>()
                    .AddScoped<SampleSystemCustomAribaLocalDBEventMessageSender>()
                    .AddKeyedSingleton("DTO", TypeResolverHelper.Create(TypeSource.FromSample<BusinessUnitSimpleDTO>(), TypeSearchMode.Both)); // For legacy audit

        private IServiceCollection AddApplicationNotification(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddNotificationJob();

            //services.AddSmtpNotification(configuration, hostEnvironment.IsProduction());

            return services;
        }
    }
}
