using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public sealed class ConditionLambda : ConditionLambdaBase<Domain.Employee>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionLambda"/> class.
    /// </summary>
    public ConditionLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = (service, versions) => true;
    }
}
