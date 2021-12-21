using System;
using System.Linq.Expressions;

using Framework.Core.Serialization;

namespace Framework.ExpressionParsers
{
    public interface INativeExpressionParser : IParser<NativeExpressionParsingData, LambdaExpression>
    {

    }

    public interface INativeExpressionParserContainer
    {
        INativeExpressionParser Parser { get; }
    }

    public abstract class NativeExpressionParserContainer : INativeExpressionParserContainer
    {
        protected NativeExpressionParserContainer(INativeExpressionParser parser)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));

            this.Parser = parser;
        }

        public INativeExpressionParser Parser { get; private set; }
    }
}