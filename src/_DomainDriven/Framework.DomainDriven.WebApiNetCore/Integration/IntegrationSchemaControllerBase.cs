﻿using System.Globalization;
using System.Net.Mime;

using Framework.Core;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Bss;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiNetCore.Integration;

[Obsolete("Will be removed in v19")]
public abstract class IntegrationSchemaControllerBase : ControllerBase
{
    private readonly TimeProvider timeProvider;

    private readonly IEventXsdExporter2 eventXsdExporter;

    private readonly IAuthorizationSystem authorizationSystem;

    private readonly SystemIntegrationRoleInfo systemIntegrationRoleInfo;

    private const string AuthIntegrationNamespace = "http://authorization.luxoft.com/IntegrationEvent";

    protected IntegrationSchemaControllerBase(
        IAuthorizationSystem authorizationSystem,
        SystemIntegrationRoleInfo systemIntegrationRoleInfo,
        TimeProvider timeProvider,
        IEventXsdExporter2 eventXsdExporter)
    {
        this.authorizationSystem = authorizationSystem;
        this.systemIntegrationRoleInfo = systemIntegrationRoleInfo;
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        this.eventXsdExporter = eventXsdExporter;
    }

    [HttpGet]
    [Route("DownloadKnownTypesWsdl")]
    public IActionResult DownloadKnownTypesWsdl() =>
            this.DownloadKnownTypesWsdl(this.IntegrationNamespace, this.GetEventDTOTypes());

    private IActionResult DownloadKnownTypesWsdl(string xsdNamespace, IReadOnlyCollection<Type> eventTypes)
    {
        this.authorizationSystem.CheckAccess(this.systemIntegrationRoleInfo.SystemIntegrationRole);

        var content = this.eventXsdExporter.Export(xsdNamespace, "IntegrationEvent", eventTypes);

        var contentType = MediaTypeNames.Application.Octet;

        var fileName =
                $"KnowTypes {xsdNamespace} ({this.timeProvider.GetToday().ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}).zip";

        return this.File(content, contentType, fileName);
    }

    [HttpGet]
    [Route("DownloadAuthKnownTypesWsdl")]
    public IActionResult DownloadAuthKnownTypesWsdl() =>
            this.DownloadKnownTypesWsdl(AuthIntegrationNamespace, this.GetAuthEventDTOTypes());

    protected abstract string IntegrationNamespace
    {
        get;
    }

    protected abstract IReadOnlyCollection<Type> GetEventDTOTypes();

    protected abstract IReadOnlyCollection<Type> GetAuthEventDTOTypes();
}
