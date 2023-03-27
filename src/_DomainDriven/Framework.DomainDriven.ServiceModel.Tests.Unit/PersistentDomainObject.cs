namespace Framework.DomainDriven.ServiceModel.Tests.Unit;

public class PersistentDomainObject
{
    private Guid id;

    private string name;

    private DateTime createdAt;

    private DateTime? modifiedAt;

    private string[] array;

    public Guid Id
    {
        get => this.id;
        set => this.id = value;
    }

    public string Name
    {
        get => this.name;
        set => this.name = value;
    }

    public DateTime CreatedAt
    {
        get => this.createdAt;
        set => this.createdAt = value;
    }

    public DateTime? ModifiedAt
    {
        get => this.modifiedAt;
        set => this.modifiedAt = value;
    }

    public string[] Array
    {
        get => this.array;
        set => this.array = value;
    }
}
