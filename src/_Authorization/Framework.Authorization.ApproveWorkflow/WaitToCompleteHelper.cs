using System;
using System.Threading;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public static class WaitToCompleteHelper
{
    public static T Retry<T>(Func<T> func, Func<T, bool> continueCondition, TimeSpan timeOut)
    {
        var res = func();
        var counter = 0;
        while (continueCondition(res) && counter < (timeOut.TotalMilliseconds / 100))
        {
            Thread.Sleep(100);
            counter++;
            res = func();
        }

        return res;
    }


    public static async Task<T> Retry<T>(Func<Task<T>> func, Func<T, bool> continueCondition, TimeSpan timeOut)
    {
        var res = await func();
        var counter = 0;
        while (continueCondition(res) && counter < (timeOut.TotalMilliseconds / 100))
        {
            Thread.Sleep(100);
            counter++;
            res = await func();
        }

        return res;
    }

    public static async Task<WorkflowStatus> WaitForWorkflowToComplete(this IPersistenceProvider persistenceProvider, string workflowId, TimeSpan timeOut)
    {
        return await Retry (() => persistenceProvider.GetStatus(workflowId), status => status == WorkflowStatus.Runnable, timeOut);
    }

    public static async Task<WorkflowStatus> GetStatus(this IPersistenceProvider persistenceProvider, string workflowId)
    {
        var instance = await persistenceProvider.GetWorkflowInstance(workflowId);

        return instance.Status;
    }
}
