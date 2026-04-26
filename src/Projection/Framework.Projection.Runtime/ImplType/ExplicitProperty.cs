using System.Reflection;

using Anch.Core;

using Framework.BLL.Domain.Persistent.Attributes;
using Framework.Core;
using Framework.Core.ReflectionImpl;
using Framework.ExtendedMetadata;

namespace Framework.Projection.ImplType;

internal class ExplicitProperty : BasePropertyInfoImpl, IWrappingObject
{
    internal readonly PropertyInfo InterfaceProp;

    private readonly string baseName;

    private readonly PropertyPath? customPropertyPath;

    private readonly PropertyMethodInfoImpl getMethod = new();


    public ExplicitProperty(PropertyInfo interfaceProp, Type reflectedType, string baseName, Type propertyType, PropertyPath? customPropertyPath = null)
    {
        this.ReflectedType = reflectedType ?? throw new ArgumentNullException(nameof(reflectedType));
        this.InterfaceProp = this.GetGenericInterfaceProp(interfaceProp ?? throw new ArgumentNullException(nameof(interfaceProp)));
        this.baseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
        this.customPropertyPath = customPropertyPath;
        this.PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
    }

    public bool CanWrap => false;

    public override Type PropertyType { get; }

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name => this.InterfaceProp.Name; // $"ExplicitProp_{this.baseProperty.Name}";


    private PropertyInfo GetGenericInterfaceProp(PropertyInfo baseProp)
    {
        if (baseProp == null) throw new ArgumentNullException(nameof(baseProp));

        if (baseProp.ReflectedType!.IsGenericType)
        {
            var genericReflectedType = baseProp.ReflectedType.GetGenericTypeDefinition();

            return genericReflectedType.GetProperty(baseProp.Name, true)!;
        }

        return baseProp;
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => (object[])this.GetInternalCustomAttributes(attributeType).SelectMany().ToArray(attributeType);

    private IEnumerable<IEnumerable<Attribute>> GetInternalCustomAttributes(Type attributeType)
    {
        if (attributeType.IsAssignableFrom(typeof(ExpandPathAttribute)))
        {
            yield return this.GetExpandPathAttributes().ToArray();
        }
    }

    public override object[] GetCustomAttributes(bool inherit) =>
        new[] { this.GetExpandPathAttributes().ToArray<Attribute>() }.SelectMany().ToArray<object>();

    private IEnumerable<ExpandPathAttribute> GetExpandPathAttributes()
    {
        yield return this.customPropertyPath.Maybe(path => new ExpandPathAttribute(path.ToString())) ?? new ExpandPathAttribute(this.baseName);
    }

    public override ParameterInfo[] GetIndexParameters() => []; // this.sourceProperty.GetIndexParameters();

    public override MethodInfo GetGetMethod(bool nonPublic) => this.getMethod;

    public override MethodInfo? GetSetMethod(bool nonPublic) => null; //new PropertyMethodInfoImpl();
}
