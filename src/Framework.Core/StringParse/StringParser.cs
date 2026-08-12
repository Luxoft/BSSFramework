using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class StringParser
{
    private readonly StringBuilder regexPatternBuilder = new();
    private int resultCount;

    public MatchResultDescription Add(StringPattern pattern)
    {
        this.Validate(pattern);
        if (!string.IsNullOrWhiteSpace(pattern.AfterThatWorlds))
        {
            var joinedSeparator = string.Format(@"\s+");
            var joinAfterThatWordsParameter = string.Join(joinedSeparator, pattern.AfterThatWorlds.Split(' '));
            this.regexPatternBuilder.AppendFormat(@".*?{0}\s*", joinAfterThatWordsParameter);
        }
        if (!string.IsNullOrWhiteSpace(pattern.Start))
        {
            this.regexPatternBuilder.Append(pattern.Start);
        }
        this.regexPatternBuilder.Append("(.*?)");
        if (null != pattern.End)
        {
            this.regexPatternBuilder.Append(pattern.End);
        }
        return new MatchResultDescription(++this.resultCount);
    }

    private void Validate(StringPattern pattern)
    {

    }

    public StringParseResult Evaluate(string inputString) => new(new Regex(this.regexPatternBuilder.ToString()).Match(inputString));
}
