using System.Reflection;

using Framework.Core.ReflectionImpl;
using Framework.ExtendedMetadata;

namespace Framework.Projection._ImplType;

internal class PropertyMethodInfoImpl(MethodInfo? baseDefinition = null) : BaseMethodInfoImpl, IWrappingObject
{
    public bool CanWrap => false;

    public override MethodInfo GetBaseDefinition() => baseDefinition;

    public override bool Equals(object obj) => ReferenceEquals(this, obj);
}
