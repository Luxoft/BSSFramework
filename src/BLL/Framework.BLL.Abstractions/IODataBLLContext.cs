using Framework.OData;
using Framework.QueryLanguage;

namespace Framework.BLL;

/// <summary>
/// Контекс системы с OData
/// </summary>
public interface IODataBLLContext : IStandardExpressionBuilderContainer, ISelectOperationParserContainer;
