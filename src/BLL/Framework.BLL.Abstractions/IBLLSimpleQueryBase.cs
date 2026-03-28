namespace Framework.BLL;

public interface IBLLSimpleQueryBase<out TDomainObject>
{
    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable();
}
