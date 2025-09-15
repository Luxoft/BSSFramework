using System.Globalization;
using System.Reflection;

namespace Framework.Core;

public class BaseFieldInfoImpl : FieldInfo
{
    public override RuntimeFieldHandle FieldHandle
    {
        get { throw new NotImplementedException(); }
    }

    public override Type FieldType
    {
        get { throw new NotImplementedException(); }
    }

    public override FieldAttributes Attributes
    {
        get { throw new NotImplementedException(); }
    }

    public override string Name
    {
        get { throw new NotImplementedException(); }
    }

    public override Type DeclaringType
    {
        get { throw new NotImplementedException(); }
    }

    public override Type ReflectedType
    {
        get { throw new NotImplementedException(); }
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
        throw new NotImplementedException();
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override object GetValue(object obj)
    {
        throw new NotImplementedException();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
