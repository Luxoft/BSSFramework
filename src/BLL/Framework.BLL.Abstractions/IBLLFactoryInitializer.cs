namespace Framework.BLL;

public interface IBLLFactoryInitializer
{
    static abstract void RegisterBLLFactory(Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection);
}
