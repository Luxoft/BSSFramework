using System.Collections.Generic;

namespace Framework.Workflow.Domain
{

    /// <summary>
    /// Интерфейс для контейнера принципалов
    /// </summary>
    /// <typeparam name="TPrincipal">Тип параметра</typeparam>
    public interface IPrincipalsContainer<out TPrincipal>
    {
        IEnumerable<TPrincipal> Principals { get; }
    }
}