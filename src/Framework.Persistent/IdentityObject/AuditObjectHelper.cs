using Framework.Core;

namespace Framework.Persistent;

public static class AuditObjectHelper
{
    public static string ModifyDatePropertyName = ExpressionHelper.Create((IAuditObject obj) => obj.ModifyDate).ToPath();

    public static string ModifyByPropertyName = ExpressionHelper.Create((IAuditObject obj) => obj.ModifiedBy).ToPath();
}
