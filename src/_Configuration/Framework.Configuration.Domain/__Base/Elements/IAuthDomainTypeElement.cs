using System;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Интерфейс элемента, ссылающегося на ID авторизационного типа
    /// </summary>
    public interface IAuthDomainTypeElement
    {
        Guid AuthDomainTypeId { get; }
    }
}