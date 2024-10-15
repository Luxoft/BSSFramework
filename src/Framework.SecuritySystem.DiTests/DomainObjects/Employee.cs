namespace Framework.SecuritySystem.DiTests;

public class Employee : PersistentDomainObjectBase
{
    public BusinessUnit BusinessUnit { get; set; }

    public BusinessUnit AltBusinessUnit { get; set; }

    public Location Location { get; set; }

    public bool TestCheckbox { get; set; }
}
