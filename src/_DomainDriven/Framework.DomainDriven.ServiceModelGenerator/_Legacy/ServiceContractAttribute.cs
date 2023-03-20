namespace System.ServiceModel;

/// <summary>Fake attribute for Generator</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
internal sealed class ServiceContractAttribute : Attribute
{
    public string ConfigurationName
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string Namespace
    {
        get;
        set;
    }
}
