using System.Linq.Expressions;

using Framework.Core.Serialization;

namespace Framework.ExpressionParsers;

public interface INativeExpressionParser : IParser<NativeExpressionParsingData, LambdaExpression>
{

}

public interface INativeExpressionParserContainer
{
    INativeExpressionParser Parser { get; }
}

public abstract class NativeExpressionParserContainer(INativeExpressionParser parser) : INativeExpressionParserContainer
{
    public INativeExpressionParser Parser { get; private set; } = parser;
}
