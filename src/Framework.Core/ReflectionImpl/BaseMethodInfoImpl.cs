using System.Globalization;
using System.Reflection;

namespace Framework.Core;

public class BaseMethodInfoImpl : MethodInfo
{
    public override object[] GetCustomAttributes(bool inherit)
    {
        throw new NotImplementedException();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override ParameterInfo[] GetParameters()
    {
        throw new NotImplementedException();
    }

    public override MethodImplAttributes GetMethodImplementationFlags()
    {
        throw new NotImplementedException();
    }

    public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override MethodInfo GetBaseDefinition()
    {
        throw new NotImplementedException();
    }

    public override ICustomAttributeProvider ReturnTypeCustomAttributes
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

    public override RuntimeMethodHandle MethodHandle
    {
        get { throw new NotImplementedException(); }
    }

    public override MethodAttributes Attributes
    {
        get { throw new NotImplementedException(); }
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }
}
