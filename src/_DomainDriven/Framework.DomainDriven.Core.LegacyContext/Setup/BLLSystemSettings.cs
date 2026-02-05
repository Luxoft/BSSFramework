using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.Setup;

public class BLLSystemSettings
{
    public Type? ValidatorDeclType { get; set; }

    public Type? ValidatorImplType { get; set; }

    public Type? ValidationMapType { get; set; }

    public Type? ValidatorCompileCacheType { get; set; }

    public required Type FactoryContainerDeclType { get; set; }

    public required Type FactoryContainerImplType { get; set; }

    public required Type SettingsType { get; set; }

    public Type? FetchRuleExpanderType { get; set; }

    public T GetSafe<T>(Expression<Func<BLLSystemSettings, T?>> expr, T? defaultValue = default)
    {
        return expr.Compile().Invoke(this) ?? defaultValue ?? throw new Exception($"{expr.GetMemberName()} not initialized");
    }
}
