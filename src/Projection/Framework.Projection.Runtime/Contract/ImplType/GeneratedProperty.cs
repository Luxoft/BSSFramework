using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Core.ReflectionImpl;
using Framework.ExtendedMetadata;
using Framework.Projection._Extensions;
using Framework.Projection._ImplType;

namespace Framework.Projection.Contract.ImplType;

internal class GeneratedProperty : BasePropertyInfoImpl, IWrappingObject
{
    private readonly ProjectionContractEnvironment environment;

    internal readonly PropertyInfo ContractProperty;

    private readonly PropertyInfo sourceProperty;

    private readonly Lazy<Type> lazyPropertyType;


    public GeneratedProperty(ProjectionContractEnvironment environment, PropertyInfo contractProperty, GeneratedType reflectedType)
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

            var elementProjectionType = (GeneratedType?)this.environment.ContractTypeResolver.TryResolve(elementType);

            var propertyProjectionType = elementProjectionType.Maybe(type => contractProperty.PropertyType.IsCollection() ? typeof(IEnumerable<>).CachedMakeGenericType(type) : type);

            return propertyProjectionType ?? contractProperty.PropertyType;
        });
    }

    public bool CanWrap => false;


    public override Type PropertyType => this.lazyPropertyType.Value;

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name => this.ContractProperty.Name;

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => (object[])new[] { this.ContractProperty, this.environment.MetadataProxyProvider.Wrap(this.sourceProperty) }.SelectMany(prop => prop.GetCustomAttributes(attributeType, inherit)).ToArray(attributeType);

    public override object[] GetCustomAttributes(bool inherit) => new[] { this.ContractProperty, this.environment.MetadataProxyProvider.Wrap(this.sourceProperty) }.SelectMany(prop => prop.GetCustomAttributes(inherit)).ToArray();

    public override ParameterInfo[] GetIndexParameters() => this.ContractProperty.GetIndexParameters();

    public override MethodInfo GetGetMethod(bool nonPublic) => new PropertyMethodInfoImpl();

    public override MethodInfo? GetSetMethod(bool nonPublic) => null;

    public override string ToString() => $"GeneratedProperty: {this.Name}";
}
