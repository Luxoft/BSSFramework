using Framework.Core;

namespace Framework.DomainDriven.BLL;

public static class TypeExtensions
{
    private static EventRoleMode GetMode(this Enum value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        switch (value.ToString())
        {
            case "Save":
                return EventRoleMode.Save;

            case "Remove":
                return EventRoleMode.Remove;

            default:
                return EventRoleMode.Other;
        }
    }
}
