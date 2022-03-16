using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Attachments.Domain
{
    /// <summary>
    /// Целевая сиcтема
    /// </summary>
    /// <remarks>
    /// Целевая сиcтема может содержать подсистемы
    /// </remarks>
    [View]
    [BLLRole]
    [Table(Name = nameof(TargetSystem), Schema = "configuration")]
    public class TargetSystem : BaseDirectory, IMaster<DomainType>
    {
        private readonly bool isMain;

        private readonly ICollection<DomainType> domainTypes = new List<DomainType>();

        /// <summary>
        /// Конструктор
        /// </summary>
        public TargetSystem()
        {
        }

        /// <summary>
        /// Коллекция доменных типов целевой системы
        /// </summary>
        [UniqueGroup]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual IEnumerable<DomainType> DomainTypes
        {
            get { return this.domainTypes; }
        }

        /// <summary>
        /// Признак главной целевой системы
        /// </summary>
        public virtual bool IsMain
        {
            get { return this.isMain; }
        }

        #region IMaster<DomainType> Members

        ICollection<DomainType> IMaster<DomainType>.Details
        {
            get { return (ICollection<DomainType>)this.DomainTypes; }
        }

        #endregion
    }
}
