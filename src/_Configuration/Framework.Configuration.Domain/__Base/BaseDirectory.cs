using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Базовый персистентный класс для типов с именем
    /// </summary>
    public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
    {

        private string name;

        /// <summary>
        /// Название типа
        /// </summary>
        [VisualIdentity]
        [Required]
        [UniqueElement]
        public virtual string Name
        {
            get { return this.name.TrimNull(); }
            set { this.name = value.TrimNull(); }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}