using System;
using System.Collections.Generic;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

using Serilog;

namespace Framework.Configuration.WebApi
{
    public partial class ConfigSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(SaveExceptionMessage))]
        public ExceptionMessageIdentityDTO SaveExceptionMessage(ExceptionMessageStrictDTO exceptionMessageStrict)
        {
            if (exceptionMessageStrict == null)
            {
                throw new ArgumentNullException(nameof(exceptionMessageStrict));
            }

            return this.Evaluate(
                DBSessionMode.Write,
                evaluateData =>
                {
                    var emptyMessage = new ExceptionMessage { IsRoot = true, IsClient = true };

                    var mappedMessage = emptyMessage.WithMap(exceptionMessageStrict, evaluateData.MappingService);

                    evaluateData.Context.Logics.ExceptionMessage.Save(mappedMessage);

                    SendMessage(evaluateData, mappedMessage);

                    return mappedMessage.ToIdentityDTO();
                });
        }

        private static void SendMessage(EvaluatedData<IConfigurationBLLContext> evaluateData, ExceptionMessage mappedMessage)
        {
            try
            {
                evaluateData.Context.ExceptionSender.Send(mappedMessage.ToException(), TransactionMessageMode.Auto);
            }
            catch (Exception e)
            {
                Log.Error(e, "ExceptionMessageService");
            }
        }
    }
}
