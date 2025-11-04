using Framework.Persistent;

namespace Framework.DomainDriven;

public static class DALChangesExtensions
{
    public static ModificationType ToModificationType(this DALObjectChangeType changeType)
    {
        switch (changeType)
        {
            case DALObjectChangeType.Created:
            case DALObjectChangeType.Updated:
                return ModificationType.Save;

            case DALObjectChangeType.Removed:
                return ModificationType.Remove;

            default:
                throw new ArgumentOutOfRangeException(nameof(changeType));
        }
    }
}
