namespace System;

public interface ICloneable<out T> : ICloneable
{
    new T Clone();
}
