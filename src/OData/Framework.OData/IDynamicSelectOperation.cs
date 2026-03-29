using System.Collections.Immutable;

using Framework.OData.QueryLanguage;

namespace Framework.OData;

public interface IDynamicSelectOperation
{
    ImmutableArray<LambdaExpression> Expands { get; }

    ImmutableArray<LambdaExpression> Selects { get; }
}
