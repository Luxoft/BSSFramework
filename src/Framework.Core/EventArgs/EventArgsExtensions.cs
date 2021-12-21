namespace System
{
    public static class EventArgsExtensions
    {
        public static EventArgs<T> ToEventArgs<T>(this T source)
        {
            return new EventArgs<T>(source);
        }
    }
}