using System.Collections.Generic;

namespace Framework.Workflow.Domain
{

    /// <summary>
    /// Интерфейс для контейнера параметров
    /// </summary>
    /// <typeparam name="TParameter">Тип параметра</typeparam>
    public interface IParametersContainer<out TParameter>
    {
        IEnumerable<TParameter> Parameters { get; }
    }
}