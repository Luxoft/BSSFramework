using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.DomainDriven.SerializeMetadata;

[DataContract]
public class SystemMetadata
{
    public SystemMetadata(IEnumerable<TypeMetadata> types)
    {
        if (types == null) throw new ArgumentNullException(nameof(types));

        this.Types = types.ToReadOnlyCollection();
    }


    [DataMember]
    public ReadOnlyCollection<TypeMetadata> Types { get; private set; }
}
