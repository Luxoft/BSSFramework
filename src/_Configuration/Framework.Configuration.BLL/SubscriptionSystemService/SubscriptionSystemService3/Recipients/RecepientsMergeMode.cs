using Framework.Subscriptions.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

/// <summary>
/// Перечисление, указывающее способ слияния списков получателей уведомлений.
/// </summary>
public enum RecipientsMergeMode
{
    /// <summary>Объединение списков</summary>
    Union = RecipientsSelectorMode.Union,

    /// <summary>Пересечение списков</summary>
    Intersect = RecipientsSelectorMode.Intersect,

    /// <summary>Левый список за исключением получателей, указанных в правом списке.</summary>
    LeftExceptRight = RecipientsSelectorMode.RolesExceptGeneration,

    /// <summary>Правый список за исключением получателей, указанных в левом списке.</summary>
    RightExceptLeft = RecipientsSelectorMode.GenerationExceptRoles
}
