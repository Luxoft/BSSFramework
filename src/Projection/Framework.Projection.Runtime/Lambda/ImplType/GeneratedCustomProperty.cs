using System.Reflection;

using Anch.Core;

using Framework.Core.ReflectionImpl;
using Framework.ExtendedMetadata;
using Framework.Projection.ImplType;

namespace Framework.Projection.Lambda.ImplType;

internal class GeneratedCustomProperty : BasePropertyInfoImpl, IWrappingObject
{
    private readonly ProjectionLambdaEnvironment environment;

    private readonly Lazy<Type> lazyPropertyType;

    private readonly IProjectionCustomProperty customProjectionProperty;

    private readonly PropertyMethodInfoImpl getMethod = new();

    private readonly PropertyMethodInfoImpl? setMethod;


    public GeneratedCustomProperty(ProjectionLambdaEnvironment environment, IProjectionCustomProperty customProperty, GeneratedType reflectedType)
    {
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        this.customProjectionProperty = customProperty ?? throw new ArgumentNullException(nameof(customProperty));

        this.ReflectedType = reflectedType ?? throw new ArgumentNullException(nameof(reflectedType));

        this.lazyPropertyType = LazyHelper.Create(() => this.environment.BuildPropertyType(this.customProjectionProperty.Type, reflectedType, this.Name));

        if (customProperty.Writable)
        {
            this.setMethod = new PropertyMethodInfoImpl();
        }
    }
    public bool CanWrap => false;

    public override Type PropertyType => this.lazyPropertyType.Value;

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name => this.customProjectionProperty.Name;


    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => (object[])this.customProjectionProperty.Attributes.Where(attributeType.IsInstanceOfType).ToArray(attributeType);

    public override object[] GetCustomAttributes(bool inherit) => this.customProjectionProperty.Attributes.ToArray<object>();

    public override ParameterInfo[] GetIndexParameters() => [];

    public override MethodInfo GetGetMethod(bool nonPublic) => this.getMethod;

    public override MethodInfo? GetSetMethod(bool nonPublic) => this.setMethod;
}
