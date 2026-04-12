using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract]
public class TypeInfoDescriptionDTO
{
    [DataMember]
    public string Namespace { get; set; }

    [DataMember]
    public string Name { get; set; }

    public TypeNameIdentity ToDomainObject() => new() { Namespace = this.Namespace, Name = this.Name };

    public override string ToString() => $"Name: {this.Name}, Namespace: {this.Namespace}";
}
