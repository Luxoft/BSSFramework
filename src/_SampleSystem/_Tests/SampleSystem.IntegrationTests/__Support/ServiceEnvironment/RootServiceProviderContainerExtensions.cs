using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.IntegrationTests;

public static class RootServiceProviderContainerExtensions
{
    public static TResult EvaluateController<TController, TResult>(this IRootServiceProviderContainer controllerEvaluator, Expression<Func<TController, TResult>> func)
            where TController : ControllerBase
    {
        return controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(func);
    }

    public static void EvaluateController<TController>(this IRootServiceProviderContainer controllerEvaluator, Expression<Action<TController>> action)
            where TController : ControllerBase
    {
        controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(action);
    }

    public static IContextEvaluator<ISampleSystemBLLContext> GetContextEvaluator(this IRootServiceProviderContainer rootServiceProviderContainer)
    {
        return rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IContextEvaluator<ISampleSystemBLLContext>>();
    }

    public static IDateTimeService GetDateTimeService(this IRootServiceProviderContainer rootServiceProviderContainer)
    {
        return rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IDateTimeService>();
    }




    public static TResult EvaluateWrite<TResult>(this IRootServiceProviderContainer rootServiceProviderContainer, Func<ISampleSystemBLLContext, TResult> func)
    {
        return rootServiceProviderContainer.GetContextEvaluator().Evaluate(DBSessionMode.Write, func);
    }

    public static void EvaluateRead(this IRootServiceProviderContainer rootServiceProviderContainer, Action<ISampleSystemBLLContext> action)
    {
        rootServiceProviderContainer.GetContextEvaluator().Evaluate(DBSessionMode.Read, action);
    }

    public static T EvaluateRead<T>(this IRootServiceProviderContainer rootServiceProviderContainer, Func<ISampleSystemBLLContext, T> action)
    {
        return rootServiceProviderContainer.GetContextEvaluator().Evaluate(DBSessionMode.Read, action);
    }

    public static void EvaluateWrite(this IRootServiceProviderContainer rootServiceProviderContainer, Action<ISampleSystemBLLContext> func)
    {
        rootServiceProviderContainer.GetContextEvaluator().Evaluate(DBSessionMode.Write, context => { func(context); return Ignore.Value; });
    }
}
