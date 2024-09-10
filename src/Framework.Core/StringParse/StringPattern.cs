using System.Reflection;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class StringPattern
{
    private readonly string _defaultEnd = "$";
    private string _start;
    private string _end;

    private string _afterThatWorlds;

    public StringPattern()
    {
    }
    public string Start
    {
        get { return this._start; }
    }
    public string End
    {
        get { return this._end ?? this._defaultEnd; }
    }

    public string AfterThatWorlds
    {
        get { return this._afterThatWorlds; }
    }


    public StringPattern WithStart(char start)
    {
        return this.WithStart(new string(new[] { start }));
    }
    public StringPattern WithStart(string start)
    {
        this.SetValue(start, () => this._start, value => this._start = value, MethodBase.GetCurrentMethod());
        return this;
    }
    public StringPattern WithEnd(string end)
    {
        this.SetValue(end, () => this._end, value => this._end = value, MethodBase.GetCurrentMethod());
        return this;
    }
    public StringPattern WithAfterThatWords(string words)
    {
        this.SetValue(words, () => this._afterThatWorlds, value => this._afterThatWorlds = value, MethodBase.GetCurrentMethod());
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
