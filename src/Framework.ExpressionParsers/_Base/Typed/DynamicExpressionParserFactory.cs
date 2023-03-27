using Framework.Core;

namespace Framework.ExpressionParsers;

public abstract class DynamicExpressionParserFactory : TypeCache, INativeExpressionParserContainer
{
    protected DynamicExpressionParserFactory(INativeExpressionParser parser)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));

        this.Parser = parser;
    }


    public INativeExpressionParser Parser { get; private set; }
}
