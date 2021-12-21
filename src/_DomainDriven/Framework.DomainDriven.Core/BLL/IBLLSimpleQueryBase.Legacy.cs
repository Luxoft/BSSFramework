using System;
using System.Linq;

namespace Framework.DomainDriven.BLL
{
    public partial interface IBLLSimpleQueryBase<out TDomainObject>
    {
        [Obsolete("Use GetUnsecureQueryable", true)]
        IQueryable<TDomainObject> GetQueryable();
    }
}