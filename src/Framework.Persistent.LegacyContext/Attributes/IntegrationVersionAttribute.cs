namespace Framework.Persistent;

/// <summary>
/// Аттрибут, описывающий интеграционную версию объекта.
/// Применяется в логике получения и обработки данных по интеграции.
/// </summary>
/// <seealso cref="IntegrationVersion.rst" />
[AttributeUsage(AttributeTargets.Property)]
public class IntegrationVersionAttribute : Attribute
{
    private ApplyIntegrationPolicy policy = ApplyIntegrationPolicy.IgnoreLessOrEqualVersion;

    public IntegrationVersionAttribute()
    {

    }

    public ApplyIntegrationPolicy IntegrationPolicy
    {
        get => this.policy;
        set => this.policy = value;
    }
}
