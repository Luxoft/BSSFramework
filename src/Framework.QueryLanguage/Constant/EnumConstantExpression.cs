using System.Runtime.Serialization;

using CommonFramework;

namespace Framework.QueryLanguage;

[DataContract]
public class EnumConstantExpression : ConstantExpression<string>
{
    public EnumConstantExpression(Enum value)
            : base(value.FromMaybe(() => new ArgumentNullException(nameof(value))).ToString())
    {

    }

    public EnumConstantExpression(string value)
            : base(value)
    {

    }

    //public override System.Type ValueType
    //{
    //    get { return typeof(Enum); }
    //}



    public override string ToString()
    {
        return $"\"{this.Value}\"";
    }
}
