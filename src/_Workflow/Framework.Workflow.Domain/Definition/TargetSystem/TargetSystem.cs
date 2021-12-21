using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Целевая сиcтема
    /// </summary>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [UniqueGroup]
    [BLLViewRole]
    public class TargetSystem : WorkflowItemBase, IMaster<DomainType>
    {
        private readonly ICollection<DomainType> domainTypes = new List<DomainType>();

        private readonly bool isBase;

        private readonly bool isMain;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="isBase">Признак целевой системы типов, содержащихся в системных библиотеках</param>
        /// <param name="isMain">Признак основной целевой системы</param>
        public TargetSystem(bool isBase, bool isMain)
        {
            this.isBase = isBase;
            this.isMain = isMain;
        }

        protected TargetSystem()
        {
        }

        /// <summary>
        /// Коллекция доменных типов целевой системы
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<DomainType> DomainTypes
        {
            get { return this.domainTypes; }
        }

        /// <summary>
        /// Признак целевой системы, которая создана для типов, содержащихся в системных библиотеках
        /// </summary>
        public virtual bool IsBase
        {
            get { return this.isBase; }
        }

        /// <summary>
        /// Признак основной системы
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