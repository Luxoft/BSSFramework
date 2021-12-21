namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Константы, определяющие возможные типовые роли воркфлоу параметров
    /// </summary>
    public enum WorkflowParameterRole
    {

        /// <summary>
        /// Неопределенный параметр
        /// </summary>
        Other,

        /// <summary>
        /// Параметр, хранящий Id объекта
        /// </summary>
        DomainObject,

        /// <summary>
        /// Параметр, хранящий название объекта
        /// </summary>
        InstanceIdentity,

        /// <summary>
        /// Параметр, хранящий описание объекта
        /// </summary>
        InstanceDescription
    }
}