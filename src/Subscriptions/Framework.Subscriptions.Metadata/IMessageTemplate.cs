namespace Framework.Subscriptions.Metadata;

public interface IMessageTemplate<in TRenderingObject>
{
    (string Subject, string Body) Render(IServiceProvider serviceProvider, IObjectsVersion<TRenderingObject> versions);
}
