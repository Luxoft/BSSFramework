using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Exceptions;
using Framework.Validation;

namespace Framework.Configuration.BLL;

public partial class DomainTypeBLL
{
    public DomainType GetByType(Type domainObjectType)
    {
        if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

        var domainTypeInfo = this.Context.GetDomainTypeInfo(domainObjectType);

        return this.GetById(domainTypeInfo.Id);
    }

    public DomainType GetByPath(string path)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        var blocks = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (blocks.Length == 1)
        {
            var domainType = this.Context.GetTargetSystemServices().ToComposite(tss => tss.TypeResolverS).Resolve(blocks[0], true);

            return this.Context.GetDomainType(domainType, true);
        }
        else if (blocks.Length == 2)
        {
            var targetSystemService = this.Context.GetTargetSystemService(blocks[0]);

            var domainType = targetSystemService.TypeResolverS.Resolve(blocks[1]);

            return this.Context.GetDomainType(domainType, true);
        }
        else
        {
            throw new System.ArgumentException("invalid block count", nameof(path));
        }
    }

    /// <inheritdoc />
    public void ForceEvent(DomainTypeEventModel eventModel)
    {
        if (eventModel == null) throw new ArgumentNullException(nameof(eventModel));

        this.Context.Validator.Validate(eventModel);

        var operation = eventModel.Operation;

        var targetSystem = operation.DomainType.TargetSystem;

        if (!targetSystem.IsRevision)
        {
            throw new BusinessLogicException($"Target system \"{targetSystem.Name}\" must be revision");
        }

        var targetSystemService = this.Context.GetPersistentTargetSystemService(targetSystem);

        foreach (var domainObjectId in eventModel.DomainObjectIdents)
        {
            targetSystemService.ForceEvent(operation, eventModel.Revision, domainObjectId);
        }
    }
}
