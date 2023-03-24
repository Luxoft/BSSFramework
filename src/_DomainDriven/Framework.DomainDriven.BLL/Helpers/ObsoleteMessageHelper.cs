namespace Framework.DomainDriven.BLL;

internal static class ObsoleteMessageHelper
{
    public const string LegacyCtorMessage = "For custom logic use property \"" + nameof(BLLServiceRoleAttribute.CustomImplementation) + "\"";
}
