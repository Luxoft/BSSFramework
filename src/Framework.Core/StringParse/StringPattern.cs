using System.Reflection;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class StringPattern
{
    private readonly string defaultEnd = "$";
    private string start;
    private string end;

    private string afterThatWorlds;

    public StringPattern()
    {
    }
    public string Start => this.start;

    public string End => this.end ?? this.defaultEnd;

    public string AfterThatWorlds => this.afterThatWorlds;

    public StringPattern WithStart(char start) => this.WithStart(new string(new[] { start }));

    public StringPattern WithStart(string start)
    {
        this.SetValue(start, () => this.start, value => this.start = value, MethodBase.GetCurrentMethod());
        return this;
    }
    public StringPattern WithEnd(string end)
    {
        this.SetValue(end, () => this.end, value => this.end = value, MethodBase.GetCurrentMethod());
        return this;
    }
    public StringPattern WithAfterThatWords(string words)
    {
        this.SetValue(words, () => this.afterThatWorlds, value => this.afterThatWorlds = value, MethodBase.GetCurrentMethod());
        return this;
    }

    private void SetValue(string value, Func<string> getValue, Action<string> setValue, MethodBase methodInfo)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"Setted value can't be empty. Method:{methodInfo.Name}");
        }
        if (!string.IsNullOrEmpty(getValue()))
        {
            throw new ArgumentException($"Parameter also initialized. Method:{methodInfo.Name}");
        }
        setValue(value);
    }
}
