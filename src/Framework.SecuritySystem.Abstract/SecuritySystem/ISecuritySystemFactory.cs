namespace Framework.SecuritySystem;

public interface ISecuritySystemFactory
{
    ISecuritySystem Create(bool withRunAs);
}
