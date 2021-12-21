//using System;
//using Framework.Persistent;

//namespace Framework.Workflow.Domain.Runtime
//{
//    public class TaskInstanceAssignee : Principal, IDetail<TaskInstance>
//    {
//        private readonly TaskInstance taskInstance;


//        protected TaskInstanceAssignee()
//        {

//        }

//        public TaskInstanceAssignee(TaskInstance taskInstance)
//        {
//            if (taskInstance == null) throw new ArgumentNullException("taskInstance");

//            this.taskInstance = taskInstance;
//            this.taskInstance.AddDetail(this);
//        }


//        public virtual TaskInstance TaskInstance
//        {
//            get { return this.taskInstance; }
//        }



//        #region IDetail<TaskInstance> Members

//        TaskInstance IDetail<TaskInstance>.Master
//        {
//            get { return this.TaskInstance; }
//        }

//        #endregion
//    }
//}