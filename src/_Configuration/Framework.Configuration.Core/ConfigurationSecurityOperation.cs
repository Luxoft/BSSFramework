using Framework.SecuritySystem;

namespace Framework.Configuration;

public static class ConfigurationSecurityOperation
{
    #region ExceptionMessage

    public static SecurityRule ExceptionMessageOpenModule { get; } = new(nameof(ExceptionMessageOpenModule)) { Description = "ExceptionMessage" };

    public static SecurityRule ExceptionMessageView { get; } = new(nameof(ExceptionMessageView)) { Description = "Can view ExceptionMessage" };

    #endregion

    #region Subscription

    public static SecurityRule SubscriptionOpenModule { get; } = new(nameof(SubscriptionOpenModule));

    public static SecurityRule SubscriptionView { get; } = new(nameof(SubscriptionView)) { Description = "Subscriptions" };

    public static SecurityRule SubscriptionEdit { get; } = new(nameof(SubscriptionEdit)) { Description = "Subscriptions" };

    #endregion

    #region SystemConstant

    public static SecurityRule SystemConstantOpenModule { get; } = new(nameof(SystemConstantOpenModule));

    public static SecurityRule SystemConstantView { get; } = new(nameof(SystemConstantView)) { Description = "View SystemConstant" };

    public static SecurityRule SystemConstantEdit { get; } = new(nameof(SystemConstantEdit)) { Description = "Edit SystemConstant" };

    #endregion

    #region Sequence

    public static SecurityRule SequenceOpenModule { get; } = new(nameof(SequenceOpenModule)) { Description = "Can open Sequence module" };

    public static SecurityRule SequenceView { get; } = new(nameof(SequenceView)) { Description = "Can view Sequence" };

    public static SecurityRule SequenceEdit { get; } = new(nameof(SequenceEdit)) { Description = "Can edit Sequence" };

    #endregion

    #region TargetSystem

    public static SecurityRule TargetSystemOpenModule { get; } = new(nameof(TargetSystemOpenModule));

    public static SecurityRule TargetSystemView { get; } = new(nameof(TargetSystemView));

    public static SecurityRule TargetSystemEdit { get; } = new(nameof(TargetSystemEdit));

    /// <summary>
    /// Операция для ручного инициирования событый
    /// </summary>
    public static SecurityRule ForceDomainTypeEvent { get; } = new(nameof(ForceDomainTypeEvent));

    #endregion

    /// <summary>
    /// Операция для форсирования создания нотификаций из модификаций хранящихся в локальной бд
    /// </summary>
    public static SecurityRule ProcessModifications { get; } = new(nameof(ProcessModifications)) { Description = "Process Modifications" };

    /// <summary>
    /// Операция для мониторинга состояния обработки очередей в утилитах (евентов, модификаций, нотификаций)
    /// </summary>
    public static SecurityRule QueueMonitoring { get; } = new(nameof(QueueMonitoring)) { Description = "Queue Monitoring" };
}
