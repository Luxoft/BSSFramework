using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Framework.Core;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public class WorkflowProcessResult
    {
        /// <summary>
        /// Выполненные переходы
        /// </summary>
        public ReadOnlyCollection<TransitionInstance> ExecutedTransitions { get; private set; }

        /// <summary>
        /// Отложенные проверки завершения параллельного воркфлоу
        /// </summary>
        public ReadOnlyCollection<WorkflowInstance> TryFinishParallelInstances { get; private set; }


        public WorkflowProcessResult([NotNull] TransitionInstance executedTransition)
            : this(new[] { executedTransition  }, Empty.TryFinishParallelInstances)
        {

        }

        public WorkflowProcessResult([NotNull] WorkflowInstance checkTryFinishParallelInstance)
            : this(Empty.ExecutedTransitions, new[] { checkTryFinishParallelInstance })
        {

        }

        public WorkflowProcessResult([NotNull] IEnumerable<TransitionInstance> executedTransitions, [NotNull] IEnumerable<WorkflowInstance> tryFinishParallelInstances)
        {
            if (executedTransitions == null) throw new ArgumentNullException(nameof(executedTransitions));
            if (tryFinishParallelInstances == null) throw new ArgumentNullException(nameof(tryFinishParallelInstances));

            this.ExecutedTransitions = executedTransitions.CheckNotNull().ToReadOnlyCollection();
            this.TryFinishParallelInstances = tryFinishParallelInstances.CheckNotNull().Distinct().ToReadOnlyCollection();
        }



        public static WorkflowProcessResult operator +([NotNull] WorkflowProcessResult v1, [NotNull] WorkflowProcessResult v2)
        {
            if (v1 == null) throw new ArgumentNullException(nameof(v1));
            if (v2 == null) throw new ArgumentNullException(nameof(v2));

            return ReferenceEquals(v1, Empty) ? v2
                 : ReferenceEquals(v2, Empty) ? v1
                 : new WorkflowProcessResult(v1.ExecutedTransitions.Concat(v2.ExecutedTransitions), v1.TryFinishParallelInstances.Concat(v2.TryFinishParallelInstances));
        }

        public static WorkflowProcessResult operator +([NotNull] WorkflowProcessResult v1, TransitionInstance v2)
        {
            if (v1 == null) throw new ArgumentNullException(nameof(v1));
            if (v2 == null) throw new ArgumentNullException(nameof(v2));

            return v1 + new WorkflowProcessResult(v2);
        }

        public static WorkflowProcessResult operator +([NotNull] WorkflowProcessResult v1, WorkflowInstance v2)
        {
            if (v1 == null) throw new ArgumentNullException(nameof(v1));
            if (v2 == null) throw new ArgumentNullException(nameof(v2));

            return v1 + new WorkflowProcessResult(v2);
        }


        public static readonly WorkflowProcessResult Empty = new WorkflowProcessResult(new TransitionInstance[0], new WorkflowInstance[0]);
    }

    public static class WorkflowProcessResultExtensions
    {
        public static WorkflowProcessResult Sum([NotNull] this IEnumerable<WorkflowProcessResult> workflowProcessResults)
        {
            if (workflowProcessResults == null) throw new ArgumentNullException(nameof(workflowProcessResults));

            return workflowProcessResults.Aggregate(WorkflowProcessResult.Empty, (v1, v2) => v1 + v2);
        }
    }
}
