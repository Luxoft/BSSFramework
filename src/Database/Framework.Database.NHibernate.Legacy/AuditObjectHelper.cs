using Anch.Core;

using Framework.Core;

namespace Framework.Database.NHibernate;

public static class AuditObjectHelper
{
    public static string ModifyDatePropertyName = ExpressionHelper.Create((IAuditObject obj) => obj.ModifyDate).ToPath();

    public static string ModifyByPropertyName = ExpressionHelper.Create((IAuditObject obj) => obj.ModifiedBy).ToPath();
}
