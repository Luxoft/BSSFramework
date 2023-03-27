namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IInitializeManager
{
    bool IsInitialize { get; }

    void InitializeOperation(Action operation);
}
