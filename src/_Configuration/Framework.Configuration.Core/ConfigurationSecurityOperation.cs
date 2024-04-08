using Framework.SecuritySystem;

namespace Framework.Configuration;

public static class ConfigurationSecurityOperation
{
    #region ExceptionMessage

    public static SecurityOperation ExceptionMessageOpenModule { get; } = new(nameof(ExceptionMessageOpenModule)) { Description = "ExceptionMessage" };

    public static SecurityOperation ExceptionMessageView { get; } = new(nameof(ExceptionMessageView)) { Description = "Can view ExceptionMessage" };

    #endregion

    #region Subscription

    public static SecurityOperation SubscriptionOpenModule { get; } = new(nameof(SubscriptionOpenModule));

    public static SecurityOperation SubscriptionView { get; } = new(nameof(SubscriptionView)) { Description = "Subscriptions" };

    public static SecurityOperation SubscriptionEdit { get; } = new(nameof(SubscriptionEdit)) { Description = "Subscriptions" };

    #endregion

    #region SystemConstant

    public static SecurityOperation SystemConstantOpenModule { get; } = new(nameof(SystemConstantOpenModule));

    public static SecurityOperation SystemConstantView { get; } = new(nameof(SystemConstantView)) { Description = "View SystemConstant" };

    public static SecurityOperation SystemConstantEdit { get; } = new(nameof(SystemConstantEdit)) { Description = "Edit SystemConstant" };

    #endregion

    #region Sequence

    public static SecurityOperation SequenceOpenModule { get; } = new(nameof(SequenceOpenModule)) { Description = "Can open Sequence module" };

    public static SecurityOperation SequenceView { get; } = new(nameof(SequenceView)) { Description = "Can view Sequence" };

    public static SecurityOperation SequenceEdit { get; } = new(nameof(SequenceEdit)) { Description = "Can edit Sequence" };

    #endregion

    #region TargetSystem

    public static SecurityOperation TargetSystemOpenModule { get; } = new(nameof(TargetSystemOpenModule));

    public static SecurityOperation TargetSystemView { get; } = new(nameof(TargetSystemView));

    public static SecurityOperation TargetSystemEdit { get; } = new(nameof(TargetSystemEdit));

    /// <summary>
    /// Операция для ручного инициирования событый
    /// </summary>
    public static SecurityOperation ForceDomainTypeEvent { get; } = new(nameof(ForceDomainTypeEvent));

    #endregion

    /// <summary>
    /// Операция для форсирования создания нотификаций из модификаций хранящихся в локальной бд
    /// </summary>
    public static SecurityOperation ProcessModifications { get; } = new(nameof(ProcessModifications)) { Description = "Process Modifications" };

    /// <summary>
    /// Операция для мониторинга состояния обработки очередей в утилитах (евентов, модификаций, нотификаций)
    /// </summary>
    public static SecurityOperation QueueMonitoring { get; } = new(nameof(QueueMonitoring)) { Description = "Queue Monitoring" };
}
