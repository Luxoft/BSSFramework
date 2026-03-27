namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.FileStore;

/// <summary>
/// Fake attribute just for generator
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class WebGetAttribute : Attribute
{
    public string UriTemplate
    {
        get;
        set;
    }
}
