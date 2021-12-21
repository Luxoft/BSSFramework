using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Потребитель DAL-евентов
    /// </summary>
    public interface IDALListener
    {
        /// <summary>
        /// Обработка евента
        /// </summary>
        /// <param name="eventArgs">Даннные о DAL-изменениях</param>
        void Process([NotNull] DALChangesEventArgs eventArgs);
    }
}
