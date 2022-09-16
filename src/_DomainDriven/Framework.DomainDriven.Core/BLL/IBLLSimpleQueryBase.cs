using System;
using System.Linq;

namespace Framework.DomainDriven.BLL
{
    public interface IBLLSimpleQueryBase<out TDomainObject>
    {
        /// <summary>
        /// Получение IQueryable без учёта безопасности
        /// </summary>
        /// <returns></returns>
        IQueryable<TDomainObject> GetUnsecureQueryable();
    }
}
