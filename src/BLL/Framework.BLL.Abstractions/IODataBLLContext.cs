using Framework.Core.Serialization;
using Framework.OData;
using Framework.OData.Parser;
using Framework.OData.QueryLanguage;
using Framework.OData.QueryLanguage.StandardExpressionBuilder;

namespace Framework.BLL;

/// <summary>
/// Контекс системы с OData
/// </summary>
public interface IODataBLLContext
{
    IStandardExpressionBuilder StandardExpressionBuilder { get; }

    IParser<string, SelectOperation> SelectOperationParser { get; }
}
