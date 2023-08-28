using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.DomainDriven.SerializeMetadata;

[DataContract]
public class EnumMetadata : TypeMetadata
{
    public EnumMetadata(TypeHeader type, bool isFlag, TypeHeader underlyingType, IEnumerable<KeyValuePair<string, string>> cases)
            : base(type, TypeRole.Enum)
    {
        if (underlyingType == null) throw new ArgumentNullException(nameof(underlyingType));
        if (cases == null) throw new ArgumentNullException(nameof(cases));

        this.IsFlag = isFlag;
        this.UnderlyingType = underlyingType;
        this.Cases = cases.ToReadOnlyCollection();
    }

    [DataMember]
    public bool IsFlag { get; private set; }

    [DataMember]
    public TypeHeader UnderlyingType { get; private set; }

    [DataMember]
    public ReadOnlyCollection<KeyValuePair<string, string>> Cases { get; private set; }

    public override TypeMetadata OverrideHeaderBase(Func<TypeHeader, TypeHeader> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new EnumMetadata(selector(this.Type), this.IsFlag, this.UnderlyingType, this.Cases);
    }
}
