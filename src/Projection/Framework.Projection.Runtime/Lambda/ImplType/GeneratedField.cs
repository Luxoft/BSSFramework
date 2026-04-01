using CommonFramework;

using Framework.Core;
using Framework.Core.ReflectionImpl;
using Framework.Projection.Lambda._Extensions;

namespace Framework.Projection.Lambda.ImplType;

internal class GeneratedField : BaseFieldInfoImpl
{
    private readonly ProjectionLambdaEnvironment environment;

    private readonly GeneratedProperty property;


    public GeneratedField(ProjectionLambdaEnvironment environment, GeneratedProperty property, GeneratedType reflectedType)
    {
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        this.property = property ?? throw new ArgumentNullException(nameof(property));

        this.ReflectedType = reflectedType;
        this.Name = this.property.Name.ToStartLowerCase();

        this.FieldType = this.property.PropertyType.IsCollection() ? typeof(ICollection<>).SafeMakeProjectionCollectionType(this.property.PropertyType.GetCollectionElementType()) : this.property.PropertyType;
    }


    public override Type FieldType { get; }

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name { get; }


    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => (object[])new object[0].ToArray(attributeType);

    public override bool IsDefined(Type attributeType, bool inherit) => this.GetCustomAttributes(attributeType, inherit).Any();

    public override string ToString() => $"GeneratedField: {this.Name}";
}
