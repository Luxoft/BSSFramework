namespace Framework.HierarchicalExpand;

public class IdentityRealTypeResolver : IRealTypeResolver
{
    public Type Resolve(Type identity)
    {
        return identity;
    }
}
