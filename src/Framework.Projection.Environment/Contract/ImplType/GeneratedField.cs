using System;
using System.Collections.Generic;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Contract;

internal class GeneratedField : BaseFieldInfoImpl
{
    private readonly ProjectionContractEnvironment environment;

    private readonly GeneratedProperty property;

    public GeneratedField([NotNull] ProjectionContractEnvironment environment, [NotNull] GeneratedProperty property, GeneratedType reflectedType)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));
        if (property == null) throw new ArgumentNullException(nameof(property));

        this.environment = environment;
        this.property = property;

        this.ReflectedType = reflectedType;
        this.Name = this.property.Name.ToStartLowerCase();

        this.FieldType = this.property.PropertyType.IsCollection() ? typeof(ICollection<>).CachedMakeGenericType(this.property.PropertyType.GetCollectionElementType()) : this.property.PropertyType;
    }

    public override Type FieldType { get; }

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name { get; }


    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return (object[])new object[0].ToArray(attributeType);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        return this.GetCustomAttributes(attributeType, inherit).AnyA();
    }
}
