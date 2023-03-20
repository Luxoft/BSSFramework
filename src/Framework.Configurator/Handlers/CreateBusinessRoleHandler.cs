using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class CreateBusinessRoleHandler : BaseWriteHandler, ICreateBusinessRoleHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public CreateBusinessRoleHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    public async Task Execute(HttpContext context)
    {
        var newRole = await this.ParseRequestBodyAsync<RequestBodyDto>(context);
        this.Create(newRole);
    }

    [SuppressMessage("SonarQube", "S2436", Justification = "It's ok. BusinessRoleOperationLink automatically link to BusinessRole")]
    private void Create(RequestBodyDto newRole)
    {
        var domainObject = new BusinessRole { Name = newRole.Name };
        var operationIds = this.authorizationBllContext.Authorization.Logics.Operation.GetListByIdents(newRole.OperationIds);
        foreach (var operation in operationIds)
        {
            new BusinessRoleOperationLink(domainObject) { Operation = operation };
        }

        this.authorizationBllContext.Authorization.Logics.BusinessRoleFactory.Create(BLLSecurityMode.Edit).Save(domainObject);
    }

    private class RequestBodyDto
    {
        public List<Guid> OperationIds
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
