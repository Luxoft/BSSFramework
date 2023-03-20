namespace System;

public class EventArgs<T> : EventArgs
{
    public EventArgs(T data)
    {
        this.Data = data;
    }

    public T Data { get; set; }

    public bool Handled { get; set; }
}
