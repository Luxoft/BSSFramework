namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Перечисление уровня делегирования
    /// </summary>
    public enum DelegatePermissionLevel
    {
        /// <summary>
        /// Делегирование запрещено
        /// </summary>
        Zero,

        /// <summary>
        /// Делегирование разрешено только один раз
        /// </summary>
        One,

        /// <summary>
        /// Делегирование может осуществляться по цепочке, от сотрудника к сотруднику, несколько раз
        /// </summary>
        Many
    }
}