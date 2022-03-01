using System;

using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.WebApiCore;

namespace SampleSystem.BLL.Test
{
    public static class ServiceEnvironmentExtensions
    {
        public static TController GetController<TController>(this SampleSystemServiceEnvironment serviceEnvironment, string principalName = null)
            where TController : ControllerBase, IApiControllerBase
        {
            var controller = (TController)Activator.CreateInstance(typeof(TController), serviceEnvironment, serviceEnvironment.CreateExceptionProcessor());

            controller.PrincipalName = principalName;

            return controller;
        }

        private static IExceptionProcessor CreateExceptionProcessor(this SampleSystemServiceEnvironment serviceEnvironment)
        {
            return new ApiControllerExceptionService<SampleSystemServiceEnvironment, ISampleSystemBLLContext>(serviceEnvironment);
        }
    }
}
