using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public interface IMessageTemplate<TRenderingObject>
    where TRenderingObject : class
{
    ValueTask<(string Subject, string Body)> Render(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions, CancellationToken ct);
}
