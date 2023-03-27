namespace Framework.DomainDriven.Generation.Domain;

/// <summary>
/// Политика генерации
/// </summary>
/// <typeparam name="TIdent"></typeparam>
public interface IGeneratePolicy<in TIdent>
{
    /// <summary>
    /// Проверка использования
    /// </summary>
    /// <param name="domainType">доменный тип</param>
    /// <param name="identity">Идентефикатор</param>
    /// <returns></returns>
    bool Used(Type domainType, TIdent identity);
}
