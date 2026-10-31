using System.Runtime.Serialization;

namespace Framework.BLL.DTOMapping.MergeItemData;

/// <summary>
/// Data about changes of objects in the collection
/// </summary>
/// <typeparam name="TValue">Element</typeparam>
/// <typeparam name="TIdentity">Element identifier</typeparam>
[DataContract(Name = "UpdateItemData{0}Of{1}", Namespace = "Framework.Persistent")]
public abstract class UpdateItemData<TValue, TIdentity>;

public static class UpdateItemData
{
    public static UpdateItemData<TValue, TIdentity> CreateSave<TValue, TIdentity>(TValue value) => new SaveItemData<TValue, TIdentity>(value);

    public static UpdateItemData<TValue, TIdentity> CreateRemove<TValue, TIdentity>(TIdentity identity) => new RemoveItemData<TValue, TIdentity>(identity);
}
