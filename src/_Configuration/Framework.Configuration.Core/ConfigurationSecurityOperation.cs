using Framework.SecuritySystem;

namespace Framework.Configuration;

public static class ConfigurationSecurityOperation
{
    public static DisabledSecurityOperation Disabled { get; } = SecurityOperation.Disabled;


    #region ExceptionMessage

    public static SecurityOperation<Guid> ExceptionMessageView { get; } = new(nameof(ExceptionMessageView), new Guid("{095CC891-4D06-436E-A884-774B4D592C8C}"));

    #endregion

    #region Subscription

    public static SecurityOperation<Guid> SubscriptionView { get; } = new(nameof(SubscriptionView), new Guid("D17C8617-0213-44BD-9776-33254C5A75D8"));

    public static SecurityOperation<Guid> SubscriptionEdit { get; } = new(nameof(SubscriptionEdit), new Guid("3C209F89-023F-44E5-B30C-CF0A20647CD6"));

    #endregion

    #region SystemConstant

    public static SecurityOperation<Guid> SystemConstantView { get; } = new(nameof(SystemConstantView), new Guid("3B348AE9-CAFC-45B2-AE8C-E26F9DC92B2E"));

    public static SecurityOperation<Guid> SystemConstantEdit { get; } = new(nameof(SystemConstantEdit), new Guid("D8B07D34-261A-4A61-9B14-A349BFA87837"));

    #endregion

    #region TargetSystem

    public static SecurityOperation<Guid> TargetSystemView { get; } = new(nameof(TargetSystemView), new Guid("{B5AC42D9-456F-40B8-BA8E-A7084E972AF7}"));

    public static SecurityOperation<Guid> TargetSystemEdit { get; } = new(nameof(TargetSystemEdit), new Guid("{5B122470-F5EF-4FFA-AC93-0EEA9B8E4464}"));

    /// <summary>
    /// Операция для ручного инициирования событый
    /// </summary>
    public static SecurityOperation<Guid> ForceDomainTypeEvent { get; } = new(nameof(ForceDomainTypeEvent), new Guid("{3106D276-575A-470F-A346-31C2FF47D517}"));

    #endregion

    #region Sequence
    public static SecurityOperation<Guid> SequenceView { get; } = new(nameof(SequenceView), new Guid("3F16CE67-E445-4627-8D52-75079B191191"));

    public static SecurityOperation<Guid> SequenceEdit { get; } = new(nameof(SequenceEdit), new Guid("3E70A766-C40A-485E-BC1B-38E6600F9F8A"));

    #endregion

    /// <summary>
    /// Отображение внутренних серверных ошибок клиенту
    /// </summary>
    public static SecurityOperation<Guid> DisplayInternalError { get; } = new(nameof(DisplayInternalError), new Guid("{AB8AFD01-40D2-48D0-B5F3-A12177B00D0D}"));

    /// <summary>
    /// Операция для форсирования создания нотификаций из модификаций хранящихся в локальной бд
    /// </summary>
    public static SecurityOperation<Guid> ProcessModifications { get; } = new(nameof(ProcessModifications), new Guid("{C1EF265C-D118-4FCE-A251-27A542787449}"));

    /// <summary>
    /// Операция для мониторинга состояния обработки очередей в утилитах (евентов, модификаций, нотификаций)
    /// </summary>
    public static SecurityOperation<Guid> QueueMonitoring { get; } = new(nameof(QueueMonitoring), new Guid("{3F17FC3B-F923-43BB-AAB7-87E5C8A2E0B3}"));
}
