using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public interface IBssFrameworkSetup<out TSelf>
{
    TSelf AddExtensions(IBssFrameworkExtension extension);

    TSelf AddExtensions<TBssFrameworkExtension>()
        where TBssFrameworkExtension : IBssFrameworkExtension, new() =>
        this.AddExtensions(new TBssFrameworkExtension());

    TSelf AddServices(Action<IServiceCollection> setupAction) => this.AddExtensions(new BssFrameworkExtension(setupAction));
}
