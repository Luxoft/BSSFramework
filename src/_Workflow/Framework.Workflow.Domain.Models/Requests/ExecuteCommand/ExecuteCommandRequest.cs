using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Persistent;
using Framework.Restriction;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.Domain
{
    public class ExecuteCommandRequest : ExecuteCommandRequestBase
    {
        public ExecuteCommandRequest(TaskInstance taskInstance, Command command, Dictionary<string, string> parameters)
        {
            if (taskInstance == null) throw new ArgumentNullException(nameof(taskInstance));
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));


            this.TaskInstance = taskInstance;
            this.Command = command;
            this.Parameters = command.ToExecuteCommandRequestParameters(parameters).ToList();
        }

        public ExecuteCommandRequest()
        {

        }


        [Required]
        public TaskInstance TaskInstance { get; set; }
    }

    public static class CommandExtensions
    {
        public static IEnumerable<ExecuteCommandRequestParameter> ToExecuteCommandRequestParameters([NotNull] this Command command, Dictionary<string, string> parameters)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            return from pair in parameters

                   let parameter = command.Parameters.GetByName(pair.Key, StringComparison.CurrentCultureIgnoreCase)

                   select new ExecuteCommandRequestParameter
                   {
                       Definition = parameter,
                       Value = pair.Value
                   };
        }
    }
}
