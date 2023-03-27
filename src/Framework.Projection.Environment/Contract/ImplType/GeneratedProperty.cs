using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Contract;

internal class GeneratedProperty : BasePropertyInfoImpl
{
    private readonly ProjectionContractEnvironment environment;

    internal readonly PropertyInfo ContractProperty;

    private readonly PropertyInfo sourceProperty;

    private readonly Lazy<Type> lazyPropertyType;


    public GeneratedProperty([NotNull] ProjectionContractEnvironment environment, [NotNull] PropertyInfo contractProperty, [NotNull] GeneratedType reflectedType)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));
        if (contractProperty == null) throw new ArgumentNullException(nameof(contractProperty));
        if (reflectedType == null) throw new ArgumentNullException(nameof(reflectedType));

        this.environment = environment;
        this.ContractProperty = contractProperty;
        this.sourceProperty = reflectedType.SourceType.GetImplementedProperty(contractProperty);

        this.ReflectedType = reflectedType;

        this.lazyPropertyType = LazyHelper.Create(() =>
                                                  {
                                                      var elementType = contractProperty.PropertyType.GetCollectionElementTypeOrSelf();

                                                      var elementProjectionType = (GeneratedType)this.environment.ContractTypeResolver.Resolve(elementType);

                                                      var propertyProjectionType = elementProjectionType.Maybe(type => contractProperty.PropertyType.IsCollection() ? typeof(IEnumerable<>).CachedMakeGenericType(type) : type);

                                                      return propertyProjectionType ?? contractProperty.PropertyType;
                                                  });
    }


    public override Type PropertyType => this.lazyPropertyType.Value;

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name => this.ContractProperty.Name;

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return (object[])new[] { this.ContractProperty, this.sourceProperty }.SelectMany(prop => prop.GetCustomAttributes(attributeType, inherit)).ToArray(attributeType);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
        return new[] { this.ContractProperty, this.sourceProperty }.SelectMany(prop => prop.GetCustomAttributes(inherit)).Cast<Attribute>().ToArray();
    }

    public override ParameterInfo[] GetIndexParameters()
    {
        return this.ContractProperty.GetIndexParameters();
    }

    public override MethodInfo GetGetMethod(bool nonPublic)
    {
        return new PropertyMethodInfoImpl();
    }

    public override MethodInfo GetSetMethod(bool nonPublic)
    {
        return null;//new PropertyMethodInfoImpl();
    }

    public override string ToString()
    {
        return $"GeneratedProperty: {this.Name}";
    }
}
