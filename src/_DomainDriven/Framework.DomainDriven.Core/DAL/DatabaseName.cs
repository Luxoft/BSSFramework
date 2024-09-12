#nullable enable

using Framework.Core;

namespace Framework.DomainDriven;

/// <summary>
/// Описание базы данных
/// </summary>
public record DatabaseName(string Name, string Schema = "dbo")
{
    public sealed override string ToString()
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
