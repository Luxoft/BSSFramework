using Framework.Core.Serialization;

namespace Framework.ExpressionParsers;

public class NativeExpressionParsingData : ParsingData<MethodTypeInfo, string>
{
    public NativeExpressionParsingData(MethodTypeInfo parsingInfo, string parsingValue) : base(parsingInfo, parsingValue)
    {

    }
}
