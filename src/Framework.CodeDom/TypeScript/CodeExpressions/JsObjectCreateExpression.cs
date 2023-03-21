using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Framework.CodeDom.TypeScript;

/// <summary>
/// Javascript object create expression
/// </summary>
public class JsObjectCreateExpression : CodePrimitiveExpression
{
    private readonly Dictionary<string, string> properties;

    public JsObjectCreateExpression(IEnumerable<string> properties)
    {
        this.properties = properties.ToList().ToDictionary(x => x, y => y);
    }

    public JsObjectCreateExpression(params string[] properties)
    {
        this.properties = new Dictionary<string, string>();
        properties.ToList().ForEach(x => this.properties.Add(x, x));
    }

    public Dictionary<string, string> Properties => this.properties;

    public void AddParameter(string key, string value)
    {
        this.properties.Add(key, value);
    }
}
