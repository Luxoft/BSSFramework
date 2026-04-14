using CommonFramework;

using Framework.Core;
using Framework.Core.ReflectionImpl;
using Framework.ExtendedMetadata;
using Framework.Projection.Extensions;

namespace Framework.Projection.Contract.ImplType;

internal class GeneratedField : BaseFieldInfoImpl, IWrappingObject
{
    private readonly ProjectionContractEnvironment environment;

    private readonly GeneratedProperty property;

    public GeneratedField(ProjectionContractEnvironment environment, GeneratedProperty property, GeneratedType reflectedType)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));
        if (property == null) throw new ArgumentNullException(nameof(property));

        this.environment = environment;
        this.property = property;

        this.ReflectedType = reflectedType;
        this.Name = this.property.Name.ToStartLowerCase();

        this.FieldType = this.property.PropertyType.IsCollection() ? typeof(ICollection<>).CachedMakeGenericType(this.property.PropertyType.GetCollectionElementType()) : this.property.PropertyType;
    }

    public bool CanWrap => false;

    public override Type FieldType { get; }

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name { get; }


    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => (object[])Array.Empty<object>().ToArray(attributeType);

    public override bool IsDefined(Type attributeType, bool inherit) => this.GetCustomAttributes(attributeType, inherit).Any();
}
