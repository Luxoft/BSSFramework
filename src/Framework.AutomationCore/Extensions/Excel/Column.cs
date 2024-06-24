namespace Automation.Extensions.Excel;

[AttributeUsage(AttributeTargets.All)]
public class Column : Attribute
{
    public string Name { get; set; }

    public int? Index { get; }

    public Column(string name) => this.Name = name;

    public Column(int index) => this.Index = index;
}
