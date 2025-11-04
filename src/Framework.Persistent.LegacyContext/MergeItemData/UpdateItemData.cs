using System.Runtime.Serialization;

namespace Framework.Persistent;

/// <summary>
/// Данные по изменению объектв в коллекции
/// </summary>
/// <typeparam name="TValue">Элемент</typeparam>
/// <typeparam name="TIdentity">Идент элемента</typeparam>
[DataContract(Name = "UpdateItemData{0}Of{1}", Namespace = "Framework.Persistent")]
public abstract class UpdateItemData<TValue, TIdentity>
{
}

public static class UpdateItemData
{
    public static UpdateItemData<TValue, TIdentity> CreateSave<TValue, TIdentity>(TValue value)
    {
        return new SaveItemData<TValue, TIdentity>(value);
    }

    public static UpdateItemData<TValue, TIdentity> CreateRemove<TValue, TIdentity>(TIdentity identity)
    {
        return new RemoveItemData<TValue, TIdentity>(identity);
    }
}
