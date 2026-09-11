using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public interface IBssFrameworkSetup<out TSelf>
    where TSelf : IBssFrameworkSetup<TSelf>
{
    TSelf AddExtension(IBssFrameworkExtension extension);

    TSelf AddExtension<TBssFrameworkExtension>()
        where TBssFrameworkExtension : IBssFrameworkExtension, new() =>
        this.AddExtension(new TBssFrameworkExtension());

    TSelf AddServices(Action<IServiceCollection> setupAction) => this.AddExtension(new BssFrameworkExtension(setupAction));
}
