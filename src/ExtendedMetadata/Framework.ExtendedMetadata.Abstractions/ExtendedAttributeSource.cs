using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;

namespace Framework.ExtendedMetadata;

public record ExtendedAttributeSource(FrozenDictionary<MemberInfo, ImmutableArray<Attribute>> ExtendedAttributes)
{
    public ExtendedAttributeSource(IEnumerable<ExtendedAttributeSource> list)
        : this(
            list.SelectMany(v => v.ExtendedAttributes).GroupBy(pair => pair.Key, pair => pair.Value)
                .ToFrozenDictionary(pair => pair.Key, pair => pair.SelectMany(v => v).ToImmutableArray()))
    {
    }
}
