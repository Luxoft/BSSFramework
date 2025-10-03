using Framework.Core;

using CommonFramework;

namespace Framework.Configuration;

/// <summary>
/// Тип подписки.
/// </summary>
[Flags]
public enum SubscriptionType
{
    //Invalid = 0,

    /// <summary>
    /// Подписка для удаления доменного объекта.
    /// </summary>
    Remove = 1,

    /// <summary>
    /// Подписка для изменения доменного объекта.
    /// </summary>
    Continue = 2,

    /// <summary>
    /// Подписка для создания доменного объекта.
    /// </summary>
    Create = 4,

    /// <summary>
    /// Подписка для создания или изменения доменного объекта.
    /// </summary>
    CreateOrContinue = Create | Continue,

    /// <summary>
    /// Подписка для изменения или удаления доменного объекта.
    /// </summary>
    ContinueOrRemove = Continue | Remove,

    /// <summary>
    /// Подписка для любого изменения доменного объекта.
    /// </summary>
    All = Create | Continue | Remove
}

public static class SubscriptionTypeHelper
{
    private static readonly Dictionary<Tuple<bool?, bool?>, SubscriptionType> Matrix = new Dictionary<Tuple<bool?, bool?>, SubscriptionType>
                                                                                       {
                                                                                               { Tuple.Create<bool?, bool?>(null , null ), SubscriptionType.Remove | SubscriptionType.Continue | SubscriptionType.Create },

                                                                                               { Tuple.Create<bool?, bool?>(true , false), SubscriptionType.Remove                                                     },
                                                                                               { Tuple.Create<bool?, bool?>(true , null ), SubscriptionType.Remove | SubscriptionType.Continue                          },
                                                                                               { Tuple.Create<bool?, bool?>(true , true ),                           SubscriptionType.Continue                          },
                                                                                               { Tuple.Create<bool?, bool?>(null , true ),                           SubscriptionType.Continue | SubscriptionType.Create },
                                                                                               { Tuple.Create<bool?, bool?>(false, true ),                                                       SubscriptionType.Create },
                                                                                       };

    public static SubscriptionType GetSubscriptionType(bool? requiredModePrev, bool? requiredModeNext)
    {
        return Matrix.GetValueOrDefault(Tuple.Create(requiredModePrev, requiredModeNext));
    }

    public static string ToFormattedProcessString(this SubscriptionType type)
    {
        var request = from subType in EnumHelper.GetValues<SubscriptionType>()
                      where type.HasFlag(subType)
                      select subType;

        return request.Join(" / ");
    }
}
