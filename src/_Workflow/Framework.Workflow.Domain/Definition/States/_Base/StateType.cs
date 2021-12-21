namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Константы, описывающие типы состояний объекта воркфлоу
    /// </summary>
    public enum StateType
    {

        /// <summary>
        /// Промежуточное условное состояние, при выполнении которого объект переходит в новое состояние
        /// </summary>
        Condition,

        /// <summary>
        /// Cостояния объекта воркфлоу с задачами
        /// </summary>
        Main,

        /// <summary>
        /// Состояние воркфлоу, с помощью которого можно описать параллельное выполнение дочерных воркфлоу
        /// </summary>
        Parallel
    }
}