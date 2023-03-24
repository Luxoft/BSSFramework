namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

/// <summary>
/// Перечисление, указывающее способ слияния списков получателей уведомлений.
/// </summary>
public enum RecepientsMergeMode
{
    /// <summary>Объединение списков</summary>
    Union = RecepientsSelectorMode.Union,

    /// <summary>Пересечение списков</summary>
    Intersect = RecepientsSelectorMode.Intersect,

    /// <summary>Левый список за исключением получателей, указанных в правом списке.</summary>
    LeftExceptRight = RecepientsSelectorMode.RolesExceptGeneration,

    /// <summary>Правый список за исключением получателей, указанных в левом списке.</summary>
    RightExceptLeft = RecepientsSelectorMode.GenerationExceptRoles
}
