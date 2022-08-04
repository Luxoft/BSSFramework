using System;

using Framework.DomainDriven.BLL.Configuration;

namespace Framework.DomainDriven.WebApiNetCore;

public interface IApiControllerPostProcessExceptionService
{
    public Exception Process(EvaluateException evaluateException, IConfigurationBLLContext configuration);
}
