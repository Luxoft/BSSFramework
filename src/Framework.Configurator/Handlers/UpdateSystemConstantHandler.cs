using System;
using System.Threading.Tasks;

using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.BLL;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class UpdateSystemConstantHandler<TBllContext> : BaseWriteHandler, IUpdateSystemConstantHandler
        where TBllContext : DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public UpdateSystemConstantHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var constantId = (string)context.Request.RouteValues["id"];
            var newValue = await this.ParseRequestBodyAsync<string>(context);

            this.Update(new Guid(constantId), newValue);
        }

        private void Update(Guid id, string newValue) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    var systemConstant = x.Configuration.Logics.SystemConstantFactory.Create(BLLSecurityMode.Edit).GetById(id, true);
                    systemConstant.Value = newValue;
                    x.Configuration.Logics.SystemConstant.Save(systemConstant);
                });
    }
}
