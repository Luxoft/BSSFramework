using Bss.Platform.Api.Documentation;
using Bss.Platform.Api.Middlewares;
using Bss.Platform.Events;
using Bss.Platform.Logging;

using Framework.Configurator;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.Setup;
using Framework.DomainDriven.WebApiNetCore;
using Framework.DomainDriven.WebApiNetCore.JsonConverter;
using Framework.DomainDriven.WebApiNetCore.Swagger;
using Framework.HangfireCore;
using Framework.NotificationCore.Jobs;

using Hangfire;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;

using SampleSystem.BLL._Command.CreateClassA.Integration;
using SampleSystem.ServiceEnvironment;
using SampleSystem.ServiceEnvironment.Jobs;
using SampleSystem.ServiceEnvironment.NHibernate;
using SampleSystem.WebApiCore.Services;

namespace SampleSystem.WebApiCore;


public static class Program
{
    private static async Task Main(string[] args)
    {
        await GenericProgram.Main(args, new SampleSystemNHibernateExtension());
    }
}
