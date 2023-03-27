using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class StringParser
{
    private readonly StringBuilder _regexPatternBuilder;
    private int _resultCount;

    public StringParser()
    {
        this._regexPatternBuilder = new StringBuilder();
    }
    public MatchResultDescription Add(StringPattern pattern)
    {
        this.Validate(pattern);
        if (!string.IsNullOrWhiteSpace(pattern.AfterThatWorlds))
        {
            var joinedSeparator = string.Format(@"\s+");
            var joinAfterThatWordsParameter = string.Join(joinedSeparator, pattern.AfterThatWorlds.Split(' '));
            this._regexPatternBuilder.AppendFormat(@".*?{0}\s*", joinAfterThatWordsParameter);
        }
        if (!string.IsNullOrWhiteSpace(pattern.Start))
        {
            this._regexPatternBuilder.Append(pattern.Start);
        }
        this._regexPatternBuilder.Append("(.*?)");
        if (null != pattern.End)
        {
            this._regexPatternBuilder.Append(pattern.End);
        }
        return new MatchResultDescription(++this._resultCount);
    }

    private void Validate(StringPattern pattern)
    {

    }

    public StringParseResult Evaluate(string inputString)
    {
        return new StringParseResult(new Regex(this._regexPatternBuilder.ToString()).Match(inputString));
    }
}
