using Framework.Application;
using Framework.Configuration.Domain;
using Framework.Validation.Extensions;

namespace Framework.Configuration.BLL;

public partial class DomainTypeBLL
{
    /// <inheritdoc />
    public async Task ForceEventAsync(DomainTypeEventModel eventModel, CancellationToken cancellationToken)
    {
        this.Context.Validator.Validate(eventModel);

        var targetSystem = eventModel.Operation.DomainType.TargetSystem;

        if (!targetSystem.IsRevision)
        {
            throw new BusinessLogicException($"Target system \"{targetSystem.Name}\" must be revision");
        }

        await this.Context.TargetSystemServices.Values.Single(tss => tss.TargetSystem == targetSystem).ForceEventAsync(eventModel, cancellationToken);
    }
}
