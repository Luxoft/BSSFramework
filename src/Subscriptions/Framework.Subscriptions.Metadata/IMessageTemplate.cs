using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public interface IMessageTemplate<TRenderingObject>
    where TRenderingObject : class
{
    (string Subject, string Body) Render(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions);
}
