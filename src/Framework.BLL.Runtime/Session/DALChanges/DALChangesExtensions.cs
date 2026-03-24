using Framework.Application.Session.DALObject;
using Framework.BLL.Domain;

namespace Framework.BLL.Session.DALChanges;

public static class DALChangesExtensions
{
    public static ModificationType ToModificationType(this DalObjectChangeType changeType)
    {
        switch (changeType)
        {
            case DalObjectChangeType.Created:
            case DalObjectChangeType.Updated:
                return ModificationType.Save;

            case DalObjectChangeType.Removed:
                return ModificationType.Remove;

            default:
                throw new ArgumentOutOfRangeException(nameof(changeType));
        }
    }
}
