﻿using System;

using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Repository
{
    public class RepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode> : IRepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode>
            where TDomainObject : class
            where TSecurityOperationCode : struct, Enum
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService;

        public RepositoryFactory(IServiceProvider serviceProvider, IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService)
        {
            this.serviceProvider = serviceProvider;
            this.domainSecurityService = domainSecurityService;
        }

        public IRepository<TDomainObject, TIdent> Create(ISecurityProvider<TDomainObject> securityProvider)
        {
            return ActivatorUtilities.CreateInstance<Repository<TDomainObject, TIdent>>(this.serviceProvider, securityProvider);
        }

        public IRepository<TDomainObject, TIdent> Create(TSecurityOperationCode securityOperationCode)
        {
            return this.Create(this.domainSecurityService.GetSecurityProvider(securityOperationCode));
        }

        public IRepository<TDomainObject, TIdent> Create(SecurityOperation<TSecurityOperationCode> securityOperation)
        {
            return this.Create(this.domainSecurityService.GetSecurityProvider(securityOperation));
        }

        public IRepository<TDomainObject, TIdent> Create(BLLSecurityMode securityMode)
        {
            return this.Create(this.domainSecurityService.GetSecurityProvider(securityMode));
        }

        public IRepository<TDomainObject, TIdent> Create()
        {
            return this.Create(this.domainSecurityService.GetSecurityProvider(BLLSecurityMode.Disabled));
        }
    }
}