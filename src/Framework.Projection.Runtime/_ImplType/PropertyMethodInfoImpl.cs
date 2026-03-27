using System.Reflection;

using Framework.Core;
using Framework.Core.ReflectionImpl;

namespace Framework.Projection._ImplType;

internal class PropertyMethodInfoImpl : BaseMethodInfoImpl
{
    private readonly MethodInfo baseDefinition;

    public PropertyMethodInfoImpl(MethodInfo baseDefinition = null)
    {
        this.baseDefinition = baseDefinition;
    }

    public override MethodInfo GetBaseDefinition()
    {
        return this.baseDefinition;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj);
    }
}
