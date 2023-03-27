using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public class DateTimeConstantExpression : ConstantExpression<DateTime>
{
    public DateTimeConstantExpression(DateTime value)
            : base(value)
    {

    }
}
