using System;

namespace Automation.Extensions.Excel;

[AttributeUsage(AttributeTargets.All)]
public class Column : Attribute
{
    public string Name { get; set; }

    public Column(string name) => this.Name = name;
}