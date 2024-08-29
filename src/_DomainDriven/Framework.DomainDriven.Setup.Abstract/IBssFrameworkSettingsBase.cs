namespace Framework.DomainDriven.Setup;

public interface IBssFrameworkSettingsBase<out TSelf>
{
    TSelf AddExtensions(IBssFrameworkExtension extension);

    TSelf AddExtensions<TBssFrameworkExtension>()
        where TBssFrameworkExtension : IBssFrameworkExtension, new() =>
        this.AddExtensions(new TBssFrameworkExtension());
}
