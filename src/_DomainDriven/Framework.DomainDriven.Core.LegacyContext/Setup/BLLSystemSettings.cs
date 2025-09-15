using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.Setup;

public class BLLSystemSettings
{
    public Type PersistentDomainObjectBaseType { get; set; }

    public Type ValidatorDeclType { get; set; }

    public Type ValidatorImplType { get; set; }

    public Type ValidationMapType { get; set; }

    public Type ValidatorCompileCacheType { get; set; }

    public Type FetchServiceType { get; set; }

    public Type FactoryContainerDeclType { get; set; }

    public Type FactoryContainerImplType { get; set; }

    public Type SettingsType { get; set; }

    public T GetSafe<T>(Expression<Func<BLLSystemSettings, T>> expr, T? defaultValue = default)
    {
        return expr.Compile().Invoke(this) ?? defaultValue ?? throw new Exception($"{expr.GetMemberName()} not initialized");
    }
}
