using Framework.SecuritySystem;

namespace Framework.Configuration;

public static class ConfigurationSecurityOperation
{
    public static DisabledSecurityOperation Disabled { get; } = SecurityOperation.Disabled;

    #region ExceptionMessage

    public static SecurityOperation<Guid> ExceptionMessageOpenModule { get; } = new(nameof(ExceptionMessageOpenModule), new Guid("d2804808-18b5-400a-ab4a-44f350a754ae")) { Description = "ExceptionMessage", IsClient = true };

    public static SecurityOperation<Guid> ExceptionMessageView { get; } = new(nameof(ExceptionMessageView), new Guid("095cc891-4d06-436e-a884-774b4d592c8c")) { Description = "Can view ExceptionMessage" };

    #endregion

    #region Subscription

    public static SecurityOperation<Guid> SubscriptionOpenModule { get; } = new(nameof(SubscriptionOpenModule), new Guid("4065a2df-ae7e-4ddb-ab94-7825daa9d30a")) { IsClient = true };

    public static SecurityOperation<Guid> SubscriptionView { get; } = new(nameof(SubscriptionView), new Guid("d17c8617-0213-44bd-9776-33254c5a75d8")) { Description = "Subscriptions" };

    public static SecurityOperation<Guid> SubscriptionEdit { get; } = new(nameof(SubscriptionEdit), new Guid("3c209f89-023f-44e5-b30c-cf0a20647cd6")) { Description = "Subscriptions" };

    #endregion

    #region SystemConstant

    public static SecurityOperation<Guid> SystemConstantOpenModule { get; } = new(nameof(SystemConstantOpenModule), new Guid("0cdca9b4-b35b-4d40-ae6b-1be59164416e")) { IsClient = true };

    public static SecurityOperation<Guid> SystemConstantView { get; } = new(nameof(SystemConstantView), new Guid("3b348ae9-cafc-45b2-ae8c-e26f9dc92b2e")) { Description = "SystemConstant" };

    public static SecurityOperation<Guid> SystemConstantEdit { get; } = new(nameof(SystemConstantEdit), new Guid("d8b07d34-261a-4a61-9b14-a349bfa87837")) { Description = "SystemConstant" };

    #endregion

    #region Sequence

    public static SecurityOperation<Guid> SequenceOpenModule { get; } = new(nameof(SequenceOpenModule), new Guid("a217f6f2-f9d9-49c7-b94f-0fd0981f0a5f")) { Description = "Can open Sequence module", IsClient = true };

    public static SecurityOperation<Guid> SequenceView { get; } = new(nameof(SequenceView), new Guid("3f16ce67-e445-4627-8d52-75079b191191")) { Description = "Can view Sequence" };

    public static SecurityOperation<Guid> SequenceEdit { get; } = new(nameof(SequenceEdit), new Guid("3e70a766-c40a-485e-bc1b-38e6600f9f8a")) { Description = "Can edit Sequence" };

    #endregion

    #region TargetSystem

    public static SecurityOperation<Guid> TargetSystemOpenModule { get; } = new(nameof(TargetSystemOpenModule), new Guid("cf3bbd62-04b7-4d8a-b53e-75f0b93f12cb")) { IsClient = true };

    public static SecurityOperation<Guid> TargetSystemView { get; } = new(nameof(TargetSystemView), new Guid("b5ac42d9-456f-40b8-ba8e-a7084e972af7"));

    public static SecurityOperation<Guid> TargetSystemEdit { get; } = new(nameof(TargetSystemEdit), new Guid("5b122470-f5ef-4ffa-ac93-0eea9b8e4464"));

    /// <summary>
    /// Операция для ручного инициирования событый
    /// </summary>
    public static SecurityOperation<Guid> ForceDomainTypeEvent { get; } = new(nameof(ForceDomainTypeEvent), new Guid("3106d276-575a-470f-a346-31c2ff47d517"));

    #endregion

    /// <summary>
    /// Операция для форсирования создания нотификаций из модификаций хранящихся в локальной бд
    /// </summary>
    public static SecurityOperation<Guid> ProcessModifications { get; } = new(nameof(ProcessModifications), new Guid("c1ef265c-d118-4fce-a251-27a542787449")) { Description = "Process Modifications", AdminHasAccess = false };

    /// <summary>
    /// Операция для мониторинга состояния обработки очередей в утилитах (евентов, модификаций, нотификаций)
    /// </summary>
    public static SecurityOperation<Guid> QueueMonitoring { get; } = new(nameof(QueueMonitoring), new Guid("3f17fc3b-f923-43bb-aab7-87e5c8a2e0b3")) { Description = "Queue Monitoring" };
}
