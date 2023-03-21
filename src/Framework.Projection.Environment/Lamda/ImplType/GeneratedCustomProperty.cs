using System;
using System.Linq;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal class GeneratedCustomProperty : BasePropertyInfoImpl
{
    private readonly ProjectionLambdaEnvironment environment;

    private readonly Lazy<Type> lazyPropertyType;

    private readonly IProjectionCustomProperty customProjectionProperty;

    private readonly PropertyMethodInfoImpl getMethod = new PropertyMethodInfoImpl();

    private readonly PropertyMethodInfoImpl setMethod;


    public GeneratedCustomProperty([NotNull] ProjectionLambdaEnvironment environment, [NotNull] IProjectionCustomProperty customProperty, [NotNull] GeneratedType reflectedType)
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

    public override Type PropertyType => this.lazyPropertyType.Value;

    public override Type ReflectedType { get; }

    public override Type DeclaringType => this.ReflectedType;

    public override string Name => this.customProjectionProperty.Name;


    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return (object[])this.customProjectionProperty.Attributes.Where(attributeType.IsInstanceOfType).ToArray(attributeType);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
        return this.customProjectionProperty.Attributes.ToArray();
    }

    public override ParameterInfo[] GetIndexParameters()
    {
        return new ParameterInfo[0];
    }

    public override MethodInfo GetGetMethod(bool nonPublic)
    {
        return this.getMethod;
    }

    public override MethodInfo GetSetMethod(bool nonPublic)
    {
        return this.setMethod;
    }
}
