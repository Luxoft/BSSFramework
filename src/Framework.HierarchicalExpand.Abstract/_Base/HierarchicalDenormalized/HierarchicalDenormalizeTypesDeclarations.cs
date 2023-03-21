using System;

namespace Framework.Persistent;

public struct HierarchicalDenormalizeTypesDeclarations
{
    public HierarchicalDenormalizeTypesDeclarations(Type ancestorToChildType, Type sourceToAncestorChildType)
            : this()
    {
        this.SourceToAncestorChildType = sourceToAncestorChildType;
        this.AncestorToChildType = ancestorToChildType;
    }

    public Type AncestorToChildType { get; }

    public Type SourceToAncestorChildType { get; }
}
