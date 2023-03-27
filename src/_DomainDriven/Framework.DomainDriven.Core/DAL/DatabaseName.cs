using Framework.Core;

namespace Framework.DomainDriven;

/// <summary>
/// Описание базы данных
/// </summary>
public class DatabaseName
{
    /// <summary>
    /// Имя базы данных
    /// </summary>
    public string Name
    {
        get;
    }

    /// <summary>
    /// Схема базы данных
    /// </summary>
    public string Schema
    {
        get;
    }

    public DatabaseName(string name, string schema = "dbo")
    {
        if (string.IsNullOrWhiteSpace(schema))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(schema));

        this.Name = name ?? string.Empty;
        this.Schema = schema;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is DatabaseName target))
        {
            return false;
        }

        return string.Equals(this.Name, target.Name, StringComparison.InvariantCultureIgnoreCase)
               && string.Equals(this.Schema, target.Schema, StringComparison.InvariantCultureIgnoreCase);
    }

    public override int GetHashCode() => this.Name.ToLower().GetHashCode() ^ this.Schema.ToLower().GetHashCode();

    public override string ToString()
    {
        if (this.Name == string.Empty)
        {
            return $"{this.Schema}";
        }

        return $"{this.Name}.{this.Schema}";
    }

    [Obsolete("Use constructor", true)]
    public static implicit operator DatabaseName(string name) => name.MaybeString(v => new DatabaseName(v));
}
