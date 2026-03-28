using System.Reflection;

using Framework.Core;
using Framework.Core.ReflectionImpl;

namespace Framework.Projection._ImplType;

internal class PropertyMethodInfoImpl(MethodInfo baseDefinition = null) : BaseMethodInfoImpl
{
    public override MethodInfo GetBaseDefinition()
    {
        return baseDefinition;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj);
    }
}
