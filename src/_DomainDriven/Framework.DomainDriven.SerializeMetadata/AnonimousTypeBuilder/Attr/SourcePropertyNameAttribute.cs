namespace Framework.DomainDriven.SerializeMetadata;

[AttributeUsage(AttributeTargets.Property)]
public class SourcePropertyNameAttribute : Attribute
{
    public SourcePropertyNameAttribute(string baseName)
    {
        if (baseName == null) throw new ArgumentNullException(nameof(baseName));

        this.BaseName = baseName;
    }

    public string BaseName { get; private set; }
}
