using CommonFramework;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Events;

namespace Framework.Configuration.BLL;

public class ConfigurationEventSystem(IConfigurationBLLContext context, IEnumerable<ITargetSystemService> targetSystems, IDomainObjectEventMetadata domainObjectEventMetadata) : IEventSystem
{
    public ITypeResolver<string> TypeResolver { get; } =

        context.Logics
               .DomainType
               .GetUnsecureQueryable()
               .Where(dt => dt.TargetSystem.IsRevision)
               .Select(dt => dt.FullTypeName)
               .ToHashSet()
               .Pipe(
                   dbDomainTypes =>
                       targetSystems.SelectMany(ts => ts.TypeResolverS.GetTypes())
                                    .Where(t => dbDomainTypes.Contains(t.FullName)))
               .Pipe(domainTypes => TypeResolverHelper.Create(new TypeSource(domainTypes), TypeSearchMode.Both));

    public IDomainObjectEventMetadata DomainObjectEventMetadata { get; } = domainObjectEventMetadata;

    public async Task ForceEventAsync(EventModel eventModel, CancellationToken cancellationToken)
    {
        var domainType = context.GetDomainType(eventModel.DomainType, true);

        var operation = domainType.EventOperations.Single(op => op.Name == eventModel.EventOperation.Name);

        var localEventModel = new DomainTypeEventModel
                    {
                        Operation = operation, DomainObjectIdents = eventModel.DomainObjectIdents.ToList(), Revision = eventModel.Revision,
                    };

        context.Logics.DomainType.ForceEvent(localEventModel);
    }
}
