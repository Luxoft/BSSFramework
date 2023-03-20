using System.Runtime.Serialization;
using Framework.Core;

namespace Framework.QueryLanguage;

[DataContract]
public class PeriodConstantExpression : ConstantExpression<Period>
{
    public PeriodConstantExpression(Period value)
            : base(value)
    {

    }

    public override string ToString()
    {
        return $"({this.Value})";
    }
}
