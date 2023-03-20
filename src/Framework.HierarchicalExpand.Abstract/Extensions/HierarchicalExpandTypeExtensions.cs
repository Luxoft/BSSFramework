namespace Framework.HierarchicalExpand;

public static class HierarchicalExpandTypeExtensions
{
    public static HierarchicalExpandType Reverse(this HierarchicalExpandType hierarchicalExpandType)
    {
        switch (hierarchicalExpandType)
        {
            case HierarchicalExpandType.Parents:
                return HierarchicalExpandType.Children;

            case HierarchicalExpandType.Children:
                return HierarchicalExpandType.Parents;

            default:
                return hierarchicalExpandType;
        }
    }
}
