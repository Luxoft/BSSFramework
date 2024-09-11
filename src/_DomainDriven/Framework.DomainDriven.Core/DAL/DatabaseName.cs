#nullable enable

using Framework.Core;

namespace Framework.DomainDriven;

/// <summary>
/// Описание базы данных
/// </summary>
public record DatabaseName()
{
    public DatabaseName(string name)
        : this() =>
        this.Name = name;

    public DatabaseName(string name, string schema)
        : this(name) =>
        this.Schema = schema;

    public string Name { get; set; } = "";

    public string Schema { get; init; } = "dbo";

    public override string ToString()
    {
        if (this.Name == string.Empty)
        {
            return $"{this.Schema}";
        }

        return $"{this.Name}.{this.Schema}";
    }

    [Obsolete("Use constructor", true)]
    public static implicit operator DatabaseName?(string? name) => name.MaybeString(v => new DatabaseName(v));
}
