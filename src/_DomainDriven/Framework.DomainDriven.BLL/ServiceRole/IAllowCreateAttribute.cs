namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Атрибут разрешающий создание объекта
    /// </summary>
    public interface IAllowCreateAttribute
    {
        /// <summary>
        /// Разрешение создавать объект
        /// </summary>
        bool AllowCreate { get; }
    }
}
