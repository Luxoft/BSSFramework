using System;
using System.Collections.Generic;

namespace Framework.StateMachine
{
    public class ActionWrapper
    {
        public object actionObject;
        private IEnumerable<Action> postActions;

        public ActionWrapper()
        {

        }

        public IEnumerable<Action> PostActions
        {
            get { return this.postActions; }
        }

        public void Register<T>(Func<T, bool> action, params Action[] postActions)
        {
            this.actionObject = action;
            this.postActions = postActions;
        }
        public Func<T, bool> GetAction<T>()
        {
            return (Func<T, bool>)this.actionObject;
        }
    }
}