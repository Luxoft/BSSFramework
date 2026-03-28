using Framework.Core;
using Framework.Database;

using ExpressionHelper = CommonFramework.ExpressionHelper;

namespace Framework.BLL.Domain.IdentityObject;

public static class AuditObjectHelper
{
    public static string ModifyDatePropertyName = ExpressionHelper.Create((IAuditObject obj) => obj.ModifyDate).ToPath();

    public static string ModifyByPropertyName = ExpressionHelper.Create((IAuditObject obj) => obj.ModifiedBy).ToPath();
}
