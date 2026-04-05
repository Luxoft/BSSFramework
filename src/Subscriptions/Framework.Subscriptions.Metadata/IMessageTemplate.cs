namespace Framework.Subscriptions.Metadata;

public interface IMessageTemplate<in TRenderingObject> : IMessageTemplate
{
    (string Subject, string Body) Render(IServiceProvider serviceProvider, IObjectsVersion<TRenderingObject> versions);

    Type IMessageTemplate.RenderingObjectType => typeof(TRenderingObject);
}

public interface IMessageTemplate
{
    Type RenderingObjectType { get; }
}
