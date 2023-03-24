using System;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class MatchResultDescription
{
    public int ResultIndex { get; private set; }
    internal MatchResultDescription(int resultIndex)
    {
        this.ResultIndex = resultIndex;
    }

}
