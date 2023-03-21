using System;
using Framework.Core;

namespace Framework.DomainDriven.BLL;

public class EventArgsWithCancel<T> : EventArgs<T>
{
    private bool cancel;

    public EventArgsWithCancel(T content)
            : base(content)
    {
    }

    public bool Cancel
    {
        get { return this.cancel; }
        set { this.cancel = value; }
    }
}
