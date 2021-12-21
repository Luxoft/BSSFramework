//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Framework.Core;
//using Framework.Workflow.Domain.Runtime;

//namespace Framework.Workflow.Domain
//{
//    public static class TaskInstanceExtensions
//    {
//        public static string GetAutoName(this IEnumerable<TaskInstance> taskInstances)
//        {
//            if (taskInstances == null) throw new ArgumentNullException("taskInstances");

//            return new[] { "Processing Workflow:" }.Concat(taskInstances.Select(ti => ti.Workflow.Name)).Join(Environment.NewLine);
//        }
//    }
//}