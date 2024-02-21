using System.Reflection;

using DotNetCore.CAP;
using DotNetCore.CAP.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleSystem.IntegrationTests.__Support.Utils
{
    public class TestConsumerServiceSelector : ConsumerServiceSelector
    {
        private readonly IServiceProvider serviceProvider;
        private readonly CapOptions _capOptions;

        public TestConsumerServiceSelector(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this._capOptions = serviceProvider.GetRequiredService<IOptions<CapOptions>>().Value;
        }

        protected override IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromInterfaceTypes(
        IServiceProvider provider)
        {
            var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

            var capSubscribeTypeInfo = typeof(ICapSubscribe).GetTypeInfo();

            using var scope = provider.CreateScope();
            var scopeProvider = scope.ServiceProvider;

            var serviceCollection = scopeProvider.GetRequiredService<IServiceCollection>();

            IEnumerable<ServiceDescriptor>? services = serviceCollection;

#if NET8_0_OR_GREATER
            var keyedSvc = serviceCollection.Where(o => o.IsKeyedService == true && (o.KeyedImplementationType != null || o.KeyedImplementationFactory != null));

            foreach (var service in keyedSvc)
            {
                var detectType = service.KeyedImplementationType ?? service.ServiceType;
                if (!capSubscribeTypeInfo.IsAssignableFrom(detectType)) continue;

                var actualType = service.KeyedImplementationType;
                if (actualType == null && service.KeyedImplementationFactory != null)
                    actualType = scopeProvider.GetRequiredKeyedService(service.ServiceType, service.ServiceKey).GetType();

                if (actualType == null) throw new NullReferenceException(nameof(service.ServiceType));

                executorDescriptorList.AddRange(this.GetTopicAttributesDescription(actualType.GetTypeInfo(), service.ServiceType.GetTypeInfo()));
            }

            services = services.Where(x => x.IsKeyedService == false);
#endif

            services = services.Where(o => o.ImplementationType != null || o.ImplementationFactory != null);
            foreach (var service in services)
            {
                var detectType = service.ImplementationType ?? service.ServiceType;
                if (!capSubscribeTypeInfo.IsAssignableFrom(detectType)) continue;

                var actualType = service.ImplementationType;
                if (actualType == null && service.ImplementationFactory != null)
                    actualType = scopeProvider.GetRequiredService(service.ServiceType).GetType();

                if (actualType == null) throw new NullReferenceException(nameof(service.ServiceType));

                executorDescriptorList.AddRange(this.GetTopicAttributesDescription(actualType.GetTypeInfo(), service.ServiceType.GetTypeInfo()));
            }

            return executorDescriptorList;
        }

        protected override IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromControllerTypes()
        {
            var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

            var types = Assembly.GetEntryAssembly()!.ExportedTypes;
            foreach (var type in types)
            {
                var typeInfo = type.GetTypeInfo();
                if (Helper.IsController(typeInfo)) executorDescriptorList.AddRange(this.GetTopicAttributesDescription(typeInfo));
            }

            return executorDescriptorList;
        }

        protected IEnumerable<ConsumerExecutorDescriptor> GetTopicAttributesDescription(TypeInfo typeInfo,
            TypeInfo? serviceTypeInfo = null)
        {
            var topicClassAttribute = typeInfo.GetCustomAttribute<TopicAttribute>(true);

            foreach (var method in typeInfo.GetRuntimeMethods())
            {
                var topicMethodAttributes = method.GetCustomAttributes<TopicAttribute>(true);

                // Ignore partial attributes when no topic attribute is defined on class.
                if (topicClassAttribute is null)
                    topicMethodAttributes = topicMethodAttributes.Where(x => !x.IsPartial && x.Name != null);

                if (!topicMethodAttributes.Any()) continue;

                foreach (var attr in topicMethodAttributes)
                {
                    this.SetSubscribeAttribute(attr);

                    var parameters = method.GetParameters()
                        .Select(parameter => new ParameterDescriptor
                        {
                            Name = parameter.Name!,
                            ParameterType = parameter.ParameterType,
                            IsFromCap = parameter.GetCustomAttributes(typeof(FromCapAttribute)).Any()
                                        || typeof(CancellationToken).IsAssignableFrom(parameter.ParameterType)
                        }).ToList();

                    yield return this.InitDescriptor(attr, method, typeInfo, serviceTypeInfo, parameters, topicClassAttribute);
                }
            }
        }

        protected override void SetSubscribeAttribute(TopicAttribute attribute)
        {
            var prefix = !string.IsNullOrEmpty(this._capOptions.GroupNamePrefix)
                ? $"{this._capOptions.GroupNamePrefix}."
                : string.Empty;
            attribute.Group = $"{prefix}{attribute.Group ?? this._capOptions.DefaultGroupName}.{this._capOptions.Version}";
        }

        private ConsumerExecutorDescriptor InitDescriptor(
            TopicAttribute attr,
            MethodInfo methodInfo,
            TypeInfo implType,
            TypeInfo? serviceTypeInfo,
            IList<ParameterDescriptor> parameters,
            TopicAttribute? classAttr = null)
        {
            var descriptor = new ConsumerExecutorDescriptor
            {
                Attribute = attr,
                ClassAttribute = classAttr,
                MethodInfo = methodInfo,
                ImplTypeInfo = implType,
                ServiceTypeInfo = serviceTypeInfo,
                Parameters = parameters,
                TopicNamePrefix = this._capOptions.TopicNamePrefix
            };

            return descriptor;
        }

    }
}
