using System.Runtime.Serialization;

namespace Framework.Persistent;

/// <summary>
/// Создание или обновление элемента
/// </summary>
/// <typeparam name="TValue">Элемент</typeparam>
/// <typeparam name="TIdentity">Идент элемента</typeparam>
[DataContract(Name = "SaveItemData{0}Of{1}", Namespace = "Framework.Persistent")]
public class SaveItemData<TValue, TIdentity> : UpdateItemData<TValue, TIdentity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="value">Сохраняемый элемент</param>
    public SaveItemData(TValue value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Сохраняемый элемент
    /// </summary>
    [DataMember]
    public TValue Value { get; private set; }
}
