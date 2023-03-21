using System;

namespace Framework.HierarchicalExpand;

public class IdentityHierarchicalRealTypeResolver : IHierarchicalRealTypeResolver
{
    public Type Resolve(Type identity)
    {
        return identity;
    }
}
