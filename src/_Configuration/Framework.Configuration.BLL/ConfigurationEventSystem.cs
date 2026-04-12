using Framework.Application.Events;
using Framework.BLL;
using Framework.BLL.Domain.Exceptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.Validation;

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
        this.ForceEventAsync(this.ToDomainTypeEventModel(eventModel), cancellationToken);

    private async Task ForceEventAsync(DomainTypeEventModel eventModel, CancellationToken cancellationToken)
    {
        context.Validator.Validate(eventModel);

        var targetSystem = eventModel.Operation.DomainType.TargetSystem;

        if (!targetSystem.IsRevision)
        {
            throw new BusinessLogicException($"Target system \"{targetSystem.Name}\" must be revision");
        }

        await context.TargetSystemServices.Values.Single(tss => tss.TargetSystem == targetSystem).ForceEventAsync(eventModel, cancellationToken);
    }

    private DomainTypeEventModel ToDomainTypeEventModel(EventModel eventModel)
    {
        var operation = context.GetDomainType(eventModel.DomainType).EventOperations.Single(op => op.Name == eventModel.EventOperation.Name);

        return new DomainTypeEventModel { Operation = operation, DomainObjectIdents = [.. eventModel.DomainObjectIdents], Revision = eventModel.Revision, };
    }
}
