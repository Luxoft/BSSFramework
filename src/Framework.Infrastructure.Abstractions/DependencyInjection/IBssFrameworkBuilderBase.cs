namespace Framework.Infrastructure.DependencyInjection;

public interface IBssFrameworkBuilderBase<out TSelf>
{
    TSelf AddExtensions(IBssFrameworkExtension extension);

    TSelf AddExtensions<TBssFrameworkExtension>()
        where TBssFrameworkExtension : IBssFrameworkExtension, new() =>
        this.AddExtensions(new TBssFrameworkExtension());
}
