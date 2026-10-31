using System.Runtime.Serialization;

namespace Framework.BLL.DTOMapping.MergeItemData;

/// <summary>
/// Removing an element from the collection
/// </summary>
/// <typeparam name="TValue">Element</typeparam>
/// <typeparam name="TIdentity">Element identifier</typeparam>
[DataContract(Name = "RemoveItemData{0}Of{1}", Namespace = "Framework.Persistent")]
public class RemoveItemData<TValue, TIdentity> : UpdateItemData<TValue, TIdentity>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="identity">Identifier of the element being removed</param>
    public RemoveItemData(TIdentity identity) => this.Identity = identity;

    /// <summary>
    /// Identifier of the element being removed
    /// </summary>
    [DataMember]
    public TIdentity Identity { get; private set; }
}
