using System;
using System.Collections.Generic;

namespace Framework.Core.Serialization;

public class ParsingData<TParsingInfo, TParsingValue> : IEquatable<ParsingData<TParsingInfo, TParsingValue>>
{
    public ParsingData(TParsingInfo parsingInfo, TParsingValue parsingValue)
    {
        this.ParsingInfo = parsingInfo;
        this.ParsingValue = parsingValue;
    }


    public TParsingInfo ParsingInfo { get; private set; }

    public TParsingValue ParsingValue { get; private set; }


    public override bool Equals(object obj)
    {
        return this.Equals(obj as ParsingData<TParsingInfo, TParsingValue>);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TParsingInfo>.Default.GetHashCode(this.ParsingInfo)
               ^ EqualityComparer<TParsingValue>.Default.GetHashCode(this.ParsingValue);
    }

    public bool Equals(ParsingData<TParsingInfo, TParsingValue> other)
    {
        return other != null && EqualityComparer<TParsingInfo>.Default.Equals(this.ParsingInfo, other.ParsingInfo)
                             && EqualityComparer<TParsingValue>.Default.Equals(this.ParsingValue, other.ParsingValue);
    }
}
