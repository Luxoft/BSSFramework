using System.Reflection;

using Framework.Core.ReflectionImpl;

namespace Framework.Projection._ImplType;

internal class PropertyMethodInfoImpl(MethodInfo baseDefinition = null) : BaseMethodInfoImpl
{
    public override MethodInfo GetBaseDefinition() => baseDefinition;

    public override bool Equals(object obj) => ReferenceEquals(this, obj);
}
