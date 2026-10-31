namespace Framework.BLL.Domain.Attributes;

/// <summary>
/// Attribute describing the integration version of an object.
/// Used in the logic of receiving and processing integration data.
/// </summary>
/// <seealso href="IntegrationVersion.rst" />
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
