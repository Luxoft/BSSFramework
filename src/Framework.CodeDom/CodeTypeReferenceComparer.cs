using System.CodeDom;

using CommonFramework;

using Framework.Core;

namespace Framework.CodeDom;

public class CodeTypeReferenceComparer : IEqualityComparer<CodeTypeReference>
{
    private CodeTypeReferenceComparer()
    {

    }


    public bool Equals(CodeTypeReference x, CodeTypeReference y)
    {
        return x == y || (x != null
                          && y != null
                          && x.BaseType == y.BaseType
                          && x.Options == y.Options
                          && x.ArrayRank == y.ArrayRank
                          && x.TypeArguments.Cast<CodeTypeReference>().SequenceEqual(y.TypeArguments.Cast<CodeTypeReference>(), this));
    }

    public int GetHashCode(CodeTypeReference obj)
    {
        return obj.Maybe(v => v.BaseType.GetHashCode());
    }


    public static readonly CodeTypeReferenceComparer Value = new CodeTypeReferenceComparer();
}
