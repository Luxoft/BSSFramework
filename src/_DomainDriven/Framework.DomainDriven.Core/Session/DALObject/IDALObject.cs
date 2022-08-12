using System;

namespace Framework.DomainDriven
{
    /// <summary>
    /// For proxy types
    /// </summary>
    public interface IDALObject
    {
        object Object { get; }

        Type Type { get; }

        /// <summary>
        /// Числовой идентификатор, определающий порядок применения/изменения/получения данной сущности
        /// </summary>
        long ApplyIndex { get; }
    }
}
