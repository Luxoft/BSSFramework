namespace Framework.Configuration.Core
{
    /// <summary>
    /// Тип изменения, произошедшего с доменным объектом.
    /// </summary>
    public enum DomainObjectChangeType
    {
        /// <summary>
        /// Тип изменения не определён.
        /// </summary>
        Unknown,

        /// <summary>
        /// Доменный объект создан.
        /// </summary>
        Create,

        /// <summary>
        /// Доменный объект изменён.
        /// </summary>
        Update,

        /// <summary>
        /// Доменный объект удалён.
        /// </summary>
        Delete
    }
}
