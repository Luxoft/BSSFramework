using System.Reflection;

using CommonFramework;

using Framework.Core.ReflectionImpl;
using Framework.ExtendedMetadata;
using Framework.Projection.ImplType;

namespace Framework.Projection.Lambda.ImplType;

internal class GeneratedProperty : BasePropertyInfoImpl, IWrappingObject
{
    private readonly ProjectionLambdaEnvironment environment;

    private readonly IProjectionProperty projectionProperty;

    //private readonly PropertyInfo sourceProperty;

    private readonly Lazy<Type> lazyPropertyType;

    private readonly PropertyMethodInfoImpl getMethod = new();


    public GeneratedProperty(ProjectionLambdaEnvironment environment, IProjectionProperty projectionProperty, GeneratedType reflectedType)
    {
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        this.projectionProperty = projectionProperty ?? throw new ArgumentNullException(nameof(projectionProperty));

        this.ReflectedType = reflectedType ?? throw new ArgumentNullException(nameof(reflectedType));

        this.lazyPropertyType = LazyHelper.Create(() => this.environment.BuildPropertyType(projectionProperty.Type, reflectedType, this.Name));

        if (this.IsIdentity)
        {
            this.getMethod = new PropertyMethodInfoImpl(this.environment.IdentityProperty.GetGetMethod()!);
        }
    }

    public bool CanWrap => false;


    public bool IsIdentity => this.projectionProperty.Name == this.environment.IdentityProperty.Name;




    public override Type PropertyType => this.lazyPropertyType.Value;

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name => this.projectionProperty.Name;


    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => (object[])this.projectionProperty.Attributes.Where(attributeType.IsInstanceOfType).ToArray(attributeType);

    public override object[] GetCustomAttributes(bool inherit) => this.projectionProperty.Attributes.ToArray<object>();

    public override ParameterInfo[] GetIndexParameters() => []; // this.sourceProperty.GetIndexParameters();

    public override MethodInfo GetGetMethod(bool nonPublic) => this.getMethod;

    public override MethodInfo? GetSetMethod(bool nonPublic) => null; //new PropertyMethodInfoImpl();

    public override string ToString() => $"GeneratedProperty: {this.Name}";
}
