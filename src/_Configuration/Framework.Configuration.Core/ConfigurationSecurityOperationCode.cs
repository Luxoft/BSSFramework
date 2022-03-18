using Framework.Security;

namespace Framework.Configuration
{
    public enum ConfigurationSecurityOperationCode
    {
        Disabled = 0,

        #region ExceptionMessage

        [SecurityOperation(SecurityOperationCode.ExceptionMessageView)]
        ExceptionMessageView,

        #endregion

        #region Subscription

        [SecurityOperation(SecurityOperationCode.SubscriptionView)]
        SubscriptionView,

        [SecurityOperation(SecurityOperationCode.SubscriptionEdit)]
        SubscriptionEdit,

        #endregion

        #region SystemConstant

        [SecurityOperation(SecurityOperationCode.SystemConstantView)]
        SystemConstantView,

        [SecurityOperation(SecurityOperationCode.SystemConstantEdit)]
        SystemConstantEdit,

        #endregion

        #region TargetSystem

        [SecurityOperation(SecurityOperationCode.TargetSystemView)]
        TargetSystemView,

        [SecurityOperation(SecurityOperationCode.TargetSystemEdit)]
        TargetSystemEdit,

        /// <summary>
        /// Операция для ручного инициирования событый
        /// </summary>
        [SecurityOperation(SecurityOperationCode.ForceDomainTypeEvent)]
        ForceDomainTypeEvent,

        #endregion

        #region Sequence

        [SecurityOperation(SecurityOperationCode.SequenceView)]
        SequenceView,

        [SecurityOperation(SecurityOperationCode.SequenceEdit)]
        SequenceEdit,

        #endregion

        #region Report

        [SecurityOperation(SecurityOperationCode.ReportView)]
        ReportView,

        [SecurityOperation(SecurityOperationCode.ReportEdit)]
        ReportEdit,

        [SecurityOperation(SecurityOperationCode.ReportGeneration)]
        ReportGeneration,

        #endregion

        /// <summary>
        /// Отображение внутренних серверных ошибок клиенту
        /// </summary>
        [SecurityOperation(SecurityOperationCode.DisplayInternalError)]
        DisplayInternalError,

        /// <summary>
        /// Операция для форсирования создания нотификаций из модификаций хранящихся в локальной бд
        /// </summary>
        [SecurityOperation(SecurityOperationCode.ProcessModifications)]
        ProcessModifications,

        /// <summary>
        /// Операция для мониторинга состояния обработки очередей в утилитах (евентов, модификаций, нотификаций)
        /// </summary>
        [SecurityOperation(SecurityOperationCode.QueueMonitoring)]
        QueueMonitoring,

        /// <summary>
        /// Операции производимые системами для интеграции (Biztalk и т.д.)
        /// </summary>
        [SecurityOperation(SecurityOperationCode.SystemIntegration)]
        SystemIntegration
    }
}
