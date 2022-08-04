using System;

using Framework.DomainDriven.BLL.Configuration;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApiControllerDebugPostProcessExceptionService : IApiControllerPostProcessExceptionService
{
    public Exception Process(EvaluateException evaluateException, IConfigurationBLLContext configuration)
    {
        if (evaluateException.ExpandedBaseException == evaluateException.BaseException)
        {
            return evaluateException;
        }

        return evaluateException.ExpandedBaseException;
    }
}
