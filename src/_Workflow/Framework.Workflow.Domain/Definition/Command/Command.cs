using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    ///  Доступное пользователю действие в системе
    /// </summary>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
    public partial class Command : SingleEventContainerBase<CommandEvent>,
        IMaster<CommandParameter>,
        IMaster<CommandRoleAccess>,
        IMaster<CommandMetadata>,

        IDetail<Task>,

        IParametersContainer<CommandParameter>,

        ITargetSystemElement<TargetSystem>,
        IRoleSource
    {
        private readonly ICollection<CommandRoleAccess> roleAccesses = new List<CommandRoleAccess>();

        private readonly ICollection<CommandParameter> parameters = new List<CommandParameter>();

        private readonly ICollection<CommandMetadata> metadatas = new List<CommandMetadata>();


        private readonly Task task;

        private WorkflowLambda executeAction;


        private int orderIndex;


        protected Command()
        {

        }

        /// <summary>
        /// Конструктор создает команду с ссылкой на задачу
        /// </summary>
        /// <param name="task">Задача</param>
        public Command(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            this.task = task;
            this.task.AddDetail(this);
        }

        /// <summary>
        /// Коллекция параметров команды
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<CommandParameter> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Коллекция метаданных команды
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<CommandMetadata> Metadatas
        {
            get { return this.metadatas; }
        }

        /// <summary>
        /// Коллекция связей между ролью и командой
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<CommandRoleAccess> RoleAccesses
        {
            get { return this.roleAccesses; }
        }

        /// <summary>
        /// Вычисляемый воркфлоу через задачу, в которой используется команда
        /// </summary>
        [ExpandPath("Task.Workflow")]
        public override Workflow Workflow
        {
            get { return this.Task.Workflow; }
        }

        /// <summary>
        /// Вычисляемая целевая система через воркфлоу, в которой применяется команда
        /// </summary>
        [ExpandPath("Workflow.TargetSystem")]
        public virtual TargetSystem TargetSystem
        {
            get { return this.Workflow.TargetSystem; }
        }

        ///// <summary>
        ///// Возвращает параметр команды с именем "PetentialApprovers"
        ///// </summary>
        ///// <returns>Название параметра</returns>
        //public virtual CommandParameter GetPetentialApproversParameter()
        //{
        //    return this.Parameters.GetByName(CommandParameter.PotentialApproversParameterName, false);
        //}

        /// <summary>
        /// Задача, в которую входит команда
        /// </summary>
        public virtual Task Task
        {
            get { return this.task; }
        }

        /// <summary>
        /// Вычисляемое состояние через задачу, в которой доступна команда
        /// </summary>
        [ExpandPath("Task.State")]
        public virtual State State
        {
            get { return this.Task.State; }
        }

        /// <summary>
        /// Лямбда, выполняющая команду
        /// </summary>
        /// <remarks>
        /// Лямбда, привязанная к команде, выполняет действия (сайд-эффекты), проверяет условия перед переходом объекта из одного состояния в другое
        /// Лямбда "Execute Action" принимает параметры в типизированном виде
        /// Выполнение действий с помощью лямбды "Execute Action", в отличие от "Transition", показывает, кем был инициирован переход, позволяет получить доступ до параметров команды и изменить их в случае необходимости
        /// </remarks>
        [WorkflowElementValidator]
        public virtual WorkflowLambda ExecuteAction
        {
            get { return this.executeAction; }
            set { this.executeAction = value; }
        }

        /// <summary>
        /// Свойство, с помощью которого можно организовать сортировку доступных команд
        /// </summary>
        /// <remarks>
        /// Например, на GUI интерфейсе
        /// </remarks>
        public virtual int OrderIndex
        {
            get { return this.orderIndex; }
            set { this.orderIndex = value; }
        }

        /// <summary>
        /// Коллекция ролей воркфлоу
        /// </summary>
        /// <returns>Коллекция ролей</returns>
        public virtual IEnumerable<Role> GetUsingRoles()
        {
            return this.RoleAccesses.Select(ca => ca.Role);
        }

        /// <summary>
        /// Создает экземляр класса "CommandEvent", который ссылается на текущую команду
        /// </summary>
        /// <returns>Событие выполнения команды</returns>
        protected override CommandEvent CreateDefaultEvent()
        {
            return new CommandEvent(this, true);
        }


        #region IMaster<CommandParameter> Members

        ICollection<CommandParameter> IMaster<CommandParameter>.Details
        {
            get { return this.parameters; }
        }

        #endregion

        #region IDetail<Task> Members

        Task IDetail<Task>.Master
        {
            get { return this.Task; }
        }

        #endregion

        #region IMaster<CommandRoleAccess> Members

        ICollection<CommandRoleAccess> IMaster<CommandRoleAccess>.Details
        {
            get { return this.roleAccesses; }
        }

        #endregion

        #region IMaster<CommandMetadata> Members

        ICollection<CommandMetadata> IMaster<CommandMetadata>.Details
        {
            get { return this.metadatas; }
        }

        #endregion
    }
}