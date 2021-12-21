using System;
using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public class EventArgsWithResult<T> : EventArgs<T>
    {

        private T result;
        public T Result
        {
            get { return this.result; }
            set { this.result = value; }
        }

        public EventArgsWithResult(T content)
            : base(content)
        {
            this.result = content;
        }
    }
}