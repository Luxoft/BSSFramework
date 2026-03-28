using System.Collections.Immutable;

using Framework.QueryLanguage;

namespace Framework.OData;

public interface IDynamicSelectOperation
{
    ImmutableArray<LambdaExpression> Expands { get; }

    ImmutableArray<LambdaExpression> Selects { get; }
}
