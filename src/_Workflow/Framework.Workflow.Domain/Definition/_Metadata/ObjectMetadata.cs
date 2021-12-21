using Framework.Core;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Метеданные
    /// </summary>
    /// <remarks>
    /// Метаданные (словарь вида "Строка-Строка") необходимы для хранения кастомной информации для любой из сущностей воркфлоу
    /// Например, можно добавить такие параметры, как цвет, название кнопки
    /// </remarks>
    public abstract partial class ObjectMetadata : WorkflowItemBase
    {
        private string value;

        /// <summary>
        /// Значение метаданных
        /// </summary>
        public virtual string Value
        {
            get { return this.value.TrimNull(); }
            set { this.value = value.TrimNull(); }
        }
    }
}