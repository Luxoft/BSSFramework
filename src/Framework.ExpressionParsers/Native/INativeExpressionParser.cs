using System.Linq.Expressions;

using Framework.Core.Serialization;

namespace Framework.ExpressionParsers.Native;

public interface INativeExpressionParser : IParser<NativeExpressionParsingData, LambdaExpression>;
