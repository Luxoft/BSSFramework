using System;

namespace Framework.Core;

[Obsolete("v10 Use ValueTuple")]
public static class TupleStruct
{
    [Obsolete("v10 Use ValueTuple")]
    public static TupleStruct<TArg1, TArg2, TArg3> Create<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
    {
        return new TupleStruct<TArg1, TArg2, TArg3>(arg1, arg2, arg3);
    }

    [Obsolete("v10 Use ValueTuple")]
    public static TupleStruct<TArg1, TArg2> Create<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
    {
        return new TupleStruct<TArg1, TArg2>(arg1, arg2);
    }
}

[Obsolete("v10 Use ValueTuple")]
public struct TupleStruct<TArg1, TArg2>
{
    public TArg1 Item1 { get; private set; }
    public TArg2 Item2 { get; private set; }
    public TupleStruct(TArg1 arg1, TArg2 arg2)
            : this()
    {
        this.Item1 = arg1;
        this.Item2 = arg2;
    }
}

[Obsolete("v10 Use ValueTuple")]
public struct TupleStruct<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
{
    public TArg1 Item1 { get; private set; }
    public TArg2 Item2 { get; private set; }
    public TArg3 Item3 { get; private set; }
    public TArg4 Item4 { get; private set; }
    public TArg5 Item5 { get; private set; }
    public TArg6 Item6 { get; private set; }

    public TupleStruct(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
            : this()
    {
        this.Item1 = arg1;
        this.Item2 = arg2;
        this.Item3 = arg3;
        this.Item4 = arg4;
        this.Item5 = arg5;
        this.Item6 = arg6;
    }
}

[Obsolete("v10 Use ValueTuple")]
public struct TupleStruct<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
{
    public TArg1 Item1 { get; private set; }
    public TArg2 Item2 { get; private set; }
    public TArg3 Item3 { get; private set; }
    public TArg4 Item4 { get; private set; }
    public TArg5 Item5 { get; private set; }
    public TArg6 Item6 { get; private set; }
    public TArg7 Item7 { get; private set; }
    public TArg8 Item8 { get; private set; }

    public TupleStruct(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8)
            : this()
    {
        this.Item1 = arg1;
        this.Item2 = arg2;
        this.Item3 = arg3;
        this.Item4 = arg4;
        this.Item5 = arg5;
        this.Item6 = arg6;
        this.Item7 = arg7;
        this.Item8 = arg8;
    }
}

[Obsolete("v10 Use ValueTuple")]
public struct TupleStruct<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
{
    public TArg1 Item1 { get; private set; }
    public TArg2 Item2 { get; private set; }
    public TArg3 Item3 { get; private set; }
    public TArg4 Item4 { get; private set; }
    public TArg5 Item5 { get; private set; }
    public TArg6 Item6 { get; private set; }
    public TArg7 Item7 { get; private set; }
    public TArg8 Item8 { get; private set; }
    public TArg9 Item9 { get; private set; }

    public TupleStruct(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9)
            : this()
    {
        this.Item1 = arg1;
        this.Item2 = arg2;
        this.Item3 = arg3;
        this.Item4 = arg4;
        this.Item5 = arg5;
        this.Item6 = arg6;
        this.Item7 = arg7;
        this.Item8 = arg8;
        this.Item9 = arg9;
    }
}

[Obsolete("v10 Use ValueTuple")]
public struct TupleStruct<TArg1, TArg2, TArg3>
{
    public TArg1 Item1 { get; private set; }
    public TArg2 Item2 { get; private set; }
    public TArg3 Item3 { get; private set; }
    public TupleStruct(TArg1 arg1, TArg2 arg2, TArg3 arg3)
            : this()
    {
        this.Item1 = arg1;
        this.Item2 = arg2;
        this.Item3 = arg3;
    }
}
