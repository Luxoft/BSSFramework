using System.Runtime.Serialization;

namespace Framework.Persistent
{
    /// <summary>
    /// Удаление элемента из коллекции
    /// </summary>
    /// <typeparam name="TValue">Элемент</typeparam>
    /// <typeparam name="TIdentity">Идент элемента</typeparam>
    [DataContract(Name = "RemoveItemData{0}Of{1}", Namespace = "Framework.Persistent")]
    public class RemoveItemData<TValue, TIdentity> : UpdateItemData<TValue, TIdentity>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="identity">Идентификатор удаляемого элемента</param>
        public RemoveItemData(TIdentity identity)
        {
            this.Identity = identity;
        }

        /// <summary>
        /// Идентификатор удаляемого элемента
        /// </summary>
        [DataMember]
        public TIdentity Identity { get; private set; }
    }
}
