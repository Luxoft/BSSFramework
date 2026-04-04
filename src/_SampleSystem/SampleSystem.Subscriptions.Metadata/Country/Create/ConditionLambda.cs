using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

/// <inheritdoc />
public sealed class ConditionLambda : ConditionLambdaBase<Domain.Country>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionLambda"/> class.
    /// </summary>
    public ConditionLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Create;
        this.Lambda = (service, versions) => true;
    }
}
