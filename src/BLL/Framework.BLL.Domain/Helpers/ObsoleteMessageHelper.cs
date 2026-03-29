using Framework.BLL.Domain.ServiceRole.Base;

namespace Framework.BLL.Domain.Helpers;

internal static class ObsoleteMessageHelper
{
    public const string LegacyCtorMessage = "For custom logic use property \"" + nameof(BLLServiceRoleAttribute.CustomImplementation) + "\"";
}
