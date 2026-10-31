namespace Framework.BLL;

public interface IBLLSimpleQueryBase<out TDomainObject>
{
    /// <summary>
    /// Getting IQueryable without security
    /// </summary>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable();
}
