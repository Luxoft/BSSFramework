using System.Collections.Generic;

using Framework.QueryLanguage;

namespace Framework.OData;

public interface IDynamicSelectOperation
{
    IEnumerable<LambdaExpression> Expands { get; }

    IEnumerable<LambdaExpression> Selects { get; }
}
