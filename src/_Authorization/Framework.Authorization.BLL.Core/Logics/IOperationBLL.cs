using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IOperationBLL
{
    IEnumerable<Operation> GetAvailableOperations();
}
