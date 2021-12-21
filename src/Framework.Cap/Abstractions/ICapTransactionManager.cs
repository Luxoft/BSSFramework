using System.Data;

namespace Framework.Cap.Abstractions;

public interface ICapTransactionManager
{
    void Enlist(IDbTransaction dbTransaction);
}
