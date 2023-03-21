using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;

using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiNetCore.Integration;

public abstract class IntegrationSchemaControllerBase : ControllerBase
{
    private readonly IDateTimeService dateTimeService;

    private readonly IAuthorizationBLLContext context;

    private const string AuthIntegrationNamespace = "http://authorization.luxoft.com/IntegrationEvent";

    protected IntegrationSchemaControllerBase(IAuthorizationBLLContext context, IDateTimeService dateTimeService)
    {
        this.context = context;
        this.dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
    }

    [HttpGet]
    [Route("DownloadKnownTypesWsdl")]
    public IActionResult DownloadKnownTypesWsdl()
    {
        return this.DownloadKnownTypesWsdl(this.IntegrationNamespace, this.GetEventDTOTypes());
    }

    private IActionResult DownloadKnownTypesWsdl(string xsdNamespace, IEnumerable<Type> eventTypes)
    {
        this.context.CheckAccess(new NonContextSecurityOperation<SecurityOperationCode>(SecurityOperationCode.SystemIntegration));

        var content = new EventXsdExporter(xsdNamespace, eventTypes).Export();

        var contentType = MediaTypeNames.Application.Octet;

        var fileName = $"KnowTypes {xsdNamespace} ({this.dateTimeService.Today.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}).zip";

        return this.File(content, contentType, fileName);
    }

    [HttpGet]
    [Route("DownloadAuthKnownTypesWsdl")]
    public IActionResult DownloadAuthKnownTypesWsdl()
    {
        return this.DownloadKnownTypesWsdl(AuthIntegrationNamespace, this.GetAuthEventDTOTypes());
    }

    protected abstract string IntegrationNamespace { get; }

    protected abstract IEnumerable<Type> GetEventDTOTypes();

    protected abstract IEnumerable<Type> GetAuthEventDTOTypes();
}
