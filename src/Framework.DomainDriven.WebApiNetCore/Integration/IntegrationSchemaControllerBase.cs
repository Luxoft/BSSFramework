using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiNetCore.Integration
{
    public abstract class IntegrationSchemaControllerBase<TServiceEnvironment, TBLLContext, TEvaluatedData> : ApiControllerBase<TServiceEnvironment, TBLLContext, TEvaluatedData>
            where TServiceEnvironment : class, IServiceEnvironment
            where TBLLContext : class, IConfigurationBLLContextContainer<IConfigurationBLLContext>, IAuthorizationBLLContextContainer<IAuthorizationBLLContextBase>
            where TEvaluatedData : EvaluatedData<TBLLContext>
    {
        private readonly IDateTimeService dateTimeService;

        private const string AuthIntegrationNamespace = "http://authorization.luxoft.com/IntegrationEvent";

        protected IntegrationSchemaControllerBase(TServiceEnvironment environment, IExceptionProcessor exceptionProcessor, [NotNull] IDateTimeService dateTimeService)
            : base(environment, exceptionProcessor)
        {
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
            return this.EvaluateRead(evaluateData =>
            {
                this.CheckAccess(evaluateData);

                var content = new EventXsdExporter(xsdNamespace, eventTypes).Export();

                var contentType = MediaTypeNames.Application.Octet;

                var fileName = $"KnowTypes {xsdNamespace} ({this.dateTimeService.Today.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}).zip";

                return this.File(content, contentType, fileName);
            });
        }

        [HttpGet]
        [Route("DownloadAuthKnownTypesWsdl")]
        public IActionResult DownloadAuthKnownTypesWsdl()
        {
            return this.DownloadKnownTypesWsdl(AuthIntegrationNamespace, this.GetAuthEventDTOTypes());
        }

        protected abstract void CheckAccess(TEvaluatedData eval);

        protected abstract string IntegrationNamespace { get; }

        protected abstract IEnumerable<Type> GetEventDTOTypes();

        protected abstract IEnumerable<Type> GetAuthEventDTOTypes();
    }
}
