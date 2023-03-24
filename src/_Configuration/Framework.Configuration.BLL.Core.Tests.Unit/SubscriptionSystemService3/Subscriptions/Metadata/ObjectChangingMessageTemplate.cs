using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;

internal sealed class ObjectChangingMessageTemplate : RazorTemplate<object>
{
    public override string Subject { get; } = "SampleSystem employee changed";

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}
