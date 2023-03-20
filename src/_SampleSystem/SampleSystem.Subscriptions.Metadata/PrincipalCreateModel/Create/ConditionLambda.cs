using Framework.Authorization.BLL;
using Framework.Configuration.SubscriptionModeling;

namespace SampleSystem.Subscriptions.Metadata.PrincipalCreateModel.Create;

public sealed class ConditionLambda : LambdaMetadata<IAuthorizationBLLContext, Framework.Authorization.Domain.PrincipalCreateModel, bool>
{
    public ConditionLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Create;
        this.Lambda = (context, versions) => true;
    }
}
