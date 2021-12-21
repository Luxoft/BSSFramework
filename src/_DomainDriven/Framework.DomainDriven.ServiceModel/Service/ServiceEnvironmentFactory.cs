using System;
using System.Linq;
using System.Reflection;
using Framework.Core;

namespace Framework.DomainDriven.ServiceModel.Service
{
    internal static class ServiceEnvironmentInitializeHelper
    {
        public static readonly object ConfigFactoryInstance = Activator.CreateInstance(Type.GetType(Properties.Settings.Default.ServiceEnvironmentFactoryType));
    }


    internal static class ServiceEnvironmentInitializeHelper<TServiceEnvironment>
         where TServiceEnvironment : IServiceEnvironment
    {
        public static readonly TServiceEnvironment ConfigServiceEnvironment = ((IFactory<TServiceEnvironment>)ServiceEnvironmentInitializeHelper.ConfigFactoryInstance).Create();
    }

    public abstract class ServiceEnvironmentFactory<TServiceEnvironment> : IFactory<TServiceEnvironment>
        where TServiceEnvironment : IServiceEnvironment
    {
        private static readonly Lazy<TServiceEnvironment> LazyConfigurationEnvironment = LazyHelper.Create(() =>
        {
            var factory = ServiceEnvironmentInitializeHelper.ConfigFactoryInstance.FromMaybe(() =>
                                                                                             $"Factory for type {typeof(TServiceEnvironment).Name} not found");

            if (!typeof(IFactory<TServiceEnvironment>).IsAccessableFrom(factory.GetType()))
            {
                throw new Exception($"Type {factory.GetType().Name} is not factory for {typeof(TServiceEnvironment).Name}");
            }


            var serviceEnvironmentFactoryType = factory.GetType().GetInterfaces().Single(t => t.IsGenericTypeImplementation(typeof(IFactory<>)) && typeof(TServiceEnvironment).IsAccessableFrom(t.GetGenericArguments()));

            var serviceEnvironmentType = serviceEnvironmentFactoryType.GetGenericArguments().Single();

            var createMethod = new Func<TServiceEnvironment>(CreateConfigEnvironmentWithInitialize<TServiceEnvironment>).Method.GetGenericMethodDefinition().MakeGenericMethod(serviceEnvironmentType);

            return (TServiceEnvironment)createMethod.Invoke(null, new object[] { });
        });


        private static TNestedServiceEnvironment CreateConfigEnvironmentWithInitialize<TNestedServiceEnvironment>()
            where TNestedServiceEnvironment : TServiceEnvironment
        {
            return ServiceEnvironmentInitializeHelper<TNestedServiceEnvironment>.ConfigServiceEnvironment;
        }


        protected ServiceEnvironmentFactory()
        {

        }


        protected abstract TServiceEnvironment Create();


        TServiceEnvironment IFactory<TServiceEnvironment>.Create()
        {
            return this.Create();
        }

        public static TServiceEnvironment ConfigurationEnvironment => LazyConfigurationEnvironment.Value;
    }
}