using Framework.Application.Events;
using Framework.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.TypeResolving;

namespace Framework.Configuration.BLL;

public class ConfigurationEventSystem(
    IConfigurationBLLContext context,
    ITargetSystemTypeResolverContainer targetSystemTypeResolverContainer,
    IDomainObjectEventMetadata domainObjectEventMetadata)
    : IEventSystem
{
    public ITypeResolver<TypeNameIdentity> TypeResolver { get; } = targetSystemTypeResolverContainer.TypeResolver;

    public IDomainObjectEventMetadata DomainObjectEventMetadata { get; } = domainObjectEventMetadata;

    public Task ForceEventAsync(EventModel eventModel, CancellationToken cancellationToken) =>
        context.Logics.DomainType.ForceEventAsync(this.ToDomainTypeEventModel(eventModel), cancellationToken);

    private DomainTypeEventModel ToDomainTypeEventModel(EventModel eventModel)
    {
        var operation = context.GetDomainType(eventModel.DomainType).EventOperations.Single(op => op.Name == eventModel.EventOperation.Name);

        return new DomainTypeEventModel { Operation = operation, DomainObjectIdents = [.. eventModel.DomainObjectIdents], Revision = eventModel.Revision, };
    }
}
