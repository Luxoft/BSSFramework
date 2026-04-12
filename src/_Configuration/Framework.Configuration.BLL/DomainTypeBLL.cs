using Framework.BLL.Domain.Exceptions;
using Framework.Configuration.Domain;
using Framework.Core.TypeResolving;
using Framework.Validation;

namespace Framework.Configuration.BLL;

public partial class DomainTypeBLL
{
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

        var targetSystemService = this.Context.GetTargetSystemService(targetSystem);

        foreach (var domainObjectId in eventModel.DomainObjectIdents)
        {
            targetSystemService.ForceEvent(operation, eventModel.Revision, domainObjectId);
        }
    }
}
