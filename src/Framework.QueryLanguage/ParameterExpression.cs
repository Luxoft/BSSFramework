using System;
using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.QueryLanguage;

[DataContract]
public class ParameterExpression : Expression
{
    public ParameterExpression(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        this.Name = name;
    }


    [DataMember]
    public string Name { get; private set; }


    public override bool Equals(object obj)
    {
        return this.Equals(obj as ParameterExpression);
    }


    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Name.GetHashCode();
    }

    protected override bool InternalEquals(Expression other)
    {
        return (other as ParameterExpression).Maybe(otherParameterExpression =>
                                                            this.Name == otherParameterExpression.Name);
    }



    public override string ToString()
    {
        return this.Name;
    }


    public static readonly ParameterExpression Default = new ParameterExpression("$arg");
}
