using System.Collections.Immutable;

using CommonFramework;

using Framework.Application.Events;
using Framework.BLL.Domain.TargetSystem;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Configuration.Domain;
using Framework.Core.TypeResolving;
using Framework.Core.TypeResolving.TypeSource;

namespace Framework.Configuration.BLL;

public class ConfigurationEventSystem(IConfigurationBLLContext context, IEnumerable<PersistentTargetSystemInfo> targetSystemInfoList, IDomainObjectEventMetadata domainObjectEventMetadata)
    : IEventSystem
{
    public ITypeResolver<string> TypeResolver { get; } =

        context.Logics
               .DomainType
               .GetUnsecureQueryable()
               .Where(dt => dt.TargetSystem.IsRevision)
               .Select(dt => dt.FullTypeName)
               .ToHashSet()
               .Pipe(dbDomainTypes =>
                         targetSystems.SelectMany(ts => ts.TypeResolverS.Types)
                                      .Where(t => dbDomainTypes.Contains(t.FullName!)))
               .Pipe(domainTypes => TypeResolverHelper.Create(new TypeSource(domainTypes.ToImmutableHashSet()), TypeSearchMode.Both));

    public IDomainObjectEventMetadata DomainObjectEventMetadata { get; } = domainObjectEventMetadata;

    public async Task ForceEventAsync(EventModel eventModel, CancellationToken cancellationToken)
    {
        var domainType = context.GetDomainType(eventModel.DomainType);

        var operation = domainType.EventOperations.Single(op => op.Name == eventModel.EventOperation.Name);

        var localEventModel = new DomainTypeEventModel { Operation = operation, DomainObjectIdents = [.. eventModel.DomainObjectIdents], Revision = eventModel.Revision, };

        context.Logics.DomainType.ForceEvent(localEventModel);
    }
}
