using System;
using System.Threading.Tasks;

namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IImpersonateService
{
    Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func);
}
