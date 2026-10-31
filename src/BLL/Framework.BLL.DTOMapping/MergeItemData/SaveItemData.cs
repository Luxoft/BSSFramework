using System.Runtime.Serialization;

namespace Framework.BLL.DTOMapping.MergeItemData;

/// <summary>
/// Creating or updating an element
/// </summary>
/// <typeparam name="TValue">Element</typeparam>
/// <typeparam name="TIdentity">Element identifier</typeparam>
[DataContract(Name = "SaveItemData{0}Of{1}", Namespace = "Framework.Persistent")]
public class SaveItemData<TValue, TIdentity> : UpdateItemData<TValue, TIdentity>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">Element to save</param>
    public SaveItemData(TValue value) => this.Value = value;

    /// <summary>
    /// Element to save
    /// </summary>
    [DataMember]
    public TValue Value { get; private set; }
}
