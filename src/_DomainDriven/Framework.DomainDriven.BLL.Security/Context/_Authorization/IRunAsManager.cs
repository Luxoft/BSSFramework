namespace Framework.DomainDriven.BLL.Security;

public interface IRunAsManager
{
    string PrincipalName { get; }


    bool IsRunningAs { get; }


    void StartRunAsUser(string principalName);

    void FinishRunAsUser();
}
