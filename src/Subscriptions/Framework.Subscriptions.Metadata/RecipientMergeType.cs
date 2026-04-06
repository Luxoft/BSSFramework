namespace Framework.Subscriptions.Metadata;

/// <summary>
/// Перечисление принципов комбинации адресатов рассылки
/// </summary>
/// <remarks>
/// Recipients Mode объясняет, как система должна получить рецепиентов в случае, когда оба типа заполнены
/// </remarks>
public enum RecipientMergeType
{
    /// <summary>Объединение списков</summary>
    Union,

    /// <summary>Общее пересечение списков</summary>
    Intersect,

    /// <summary>Левый список за исключением получателей, указанных в правом списке.</summary>
    LeftExceptRight,

    /// <summary>Правый список за исключением получателей, указанных в левом списке.</summary>
    RightExceptLeft
}
