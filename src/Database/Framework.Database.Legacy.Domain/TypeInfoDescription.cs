namespace Framework.Database.Domain;

public class TypeInfoDescription
{
    public TypeInfoDescription()
    {
    }

    public TypeInfoDescription(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.Name = type.Name;
        this.NameSpace = type.Namespace!;
    }


    public string NameSpace { get; set; }

    public string Name { get; set; }


    public override string ToString() => string.IsNullOrWhiteSpace(this.NameSpace) ? this.Name : $"{this.NameSpace}.{this.Name}";
}
