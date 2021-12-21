using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Описание доменного типа целевой системы
    /// </summary>
    [WorkflowViewDomainObject]
    [BLLViewRole]
    public partial class DomainType : WorkflowItemBase, ITargetSystemElement<TargetSystem>, IDetail<TargetSystem>, IDomainType
    {
        private readonly ICollection<WorkflowSource> workflowSources = new List<WorkflowSource>();


        private readonly TargetSystem targetSystem;


        private DomainTypeRole role;

        private string nameSpace;

        //private bool hasWorkflow;


        protected DomainType()
        {

        }

        /// <summary>
        /// Конструктор доменного типа целовой системы  с ссылкой на целевую систему
        /// </summary>
        /// <param name="targetSystem">Целевая система</param>
        public DomainType(TargetSystem targetSystem)
        {
            if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

            this.targetSystem = targetSystem;
            this.targetSystem.AddDetail(this);
        }

        /// <summary>
        /// Фильтр поиска экземпляра воркфлоу по доменному объекту
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual IEnumerable<WorkflowSource> WorkflowSources
        {
            get { return this.workflowSources; }
        }

        /// <summary>
        /// Целевая система
        /// </summary>
        public virtual TargetSystem TargetSystem
        {
            get { return this.targetSystem; }
        }

        /// <summary>
        /// Тип доменного объекта, связанный с ролью
        /// </summary>
        public virtual DomainTypeRole Role
        {
            get { return this.role; }
            internal protected set { this.role = value; }
        }

        //public virtual bool HasWorkflow
        //{
        //    get { return this.hasWorkflow; }
        //    internal protected set { this.hasWorkflow = value; }
        //}

        /// <summary>
        /// Пространство имен
        /// </summary>
        [UniqueElement]
        public virtual string NameSpace
        {
            get { return this.nameSpace.TrimNull(); }
            set { this.nameSpace = value.TrimNull(); }
        }

        /// <summary>
        /// Полное имя типа
        /// </summary>
        public virtual string FullTypeName
        {
            get
            {
                return string.IsNullOrEmpty(this.NameSpace)
                     ? this.Name
                     : $"{this.NameSpace}.{this.Name}";
            }
        }

        #region IDetail<TargetSystem> Members

        TargetSystem IDetail<TargetSystem>.Master
        {
            get { return this.TargetSystem; }
        }

        #endregion
    }
}