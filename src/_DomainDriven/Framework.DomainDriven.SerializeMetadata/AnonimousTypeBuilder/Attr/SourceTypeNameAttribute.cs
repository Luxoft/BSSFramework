namespace Framework.DomainDriven.SerializeMetadata;

[AttributeUsage(AttributeTargets.Class)]
public class SourceTypeNameAttribute : Attribute
{
    public SourceTypeNameAttribute(string baseName)
    {
        if (baseName == null) throw new ArgumentNullException(nameof(baseName));

        this.BaseName = baseName;
    }

    public string BaseName { get; private set; }
}
