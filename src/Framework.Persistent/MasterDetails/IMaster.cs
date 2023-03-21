using System.Collections.Generic;

namespace Framework.Persistent;

/// <summary>
/// Описывает мастер-объект в связке мастер-деталь
/// </summary>
/// <typeparam name="TDetail">Тип детали</typeparam>
public interface IMaster<TDetail>
{
    /// <summary>
    /// Ссылка на коллекцию деталей
    /// </summary>
    ICollection<TDetail> Details { get; }
}
