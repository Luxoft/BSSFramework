using Framework.OData;
using Framework.QueryLanguage;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Контекс системы с OData
/// </summary>
public interface IODataBLLContext : IStandartExpressionBuilderContainer, ISelectOperationParserContainer
{
    /// <summary>
    /// Разрешение использовать виртуальные свойства в OData-запросах
    /// </summary>
    /// <param name="domainType">Доменный тип</param>
    /// <returns></returns>
    bool AllowVirtualPropertyInOdata(Type domainType);
}
