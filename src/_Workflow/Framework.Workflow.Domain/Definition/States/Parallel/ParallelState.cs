using System.Collections.Generic;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Состояние воркфлоу, с помощью которого можно описать параллельное выполнение дочерных воркфлоу
    /// </summary>
    /// <remarks>
    /// Особенности параллельного состояния:
    /// 1) У воркфлоу есть несколько дочерних воркфлоу
    /// 2) Дочерние воркфлоу выполняются независимо друг от друга
    /// 3) Экземпляр воркфлоу выйдет из параллельного состояния, когда все внутренние дочерние воркфлоу завершатся успешно, либо один из них будет отменен
    /// </remarks>
    public class ParallelState : StateBase,
        IMaster<ParallelStateStartItem>,
        IMaster<ParallelStateFinalEvent>
    {
        private readonly ICollection<ParallelStateStartItem> startItems = new List<ParallelStateStartItem>();

        private readonly ICollection<ParallelStateFinalEvent> finalEvents = new List<ParallelStateFinalEvent>();

        /// <summary>
        /// Конструктор создает паралелльное состояние с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public ParallelState(Workflow workflow)
            : base (workflow, StateType.Parallel)
        {
            workflow.AddDetail(this);
        }

        protected ParallelState()
        {
        }

        /// <summary>
        /// Коллекция связей между воркфлоу и дочерними воркфлоу
        /// </summary>
        public virtual IEnumerable<ParallelStateStartItem> StartItems
        {
            get { return this.startItems; }
        }

        /// <summary>
        /// Коллекция событий при выходе из параллельного состояния
        /// </summary>
        /// <remarks>
        /// События с положительным/отрицательным выходом
        /// </remarks>
        public virtual IEnumerable<ParallelStateFinalEvent> FinalEvents
        {
            get { return this.finalEvents; }
        }

        #region IMaster<ParallelStateItem> Members

        ICollection<ParallelStateStartItem> IMaster<ParallelStateStartItem>.Details
        {
            get { return (ICollection<ParallelStateStartItem>)this.StartItems; }
        }

        ICollection<ParallelStateFinalEvent> IMaster<ParallelStateFinalEvent>.Details
        {
            get { return (ICollection<ParallelStateFinalEvent>)this.FinalEvents; }
        }

        #endregion
    }
}