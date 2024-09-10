namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class TestMessageTemplate : RazorTemplate<object>
{
    public override string Subject { get; } = "SampleSystem employee changed";

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
