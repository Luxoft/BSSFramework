﻿using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLContext<out TAuthorizationBLLContext, in TPersistentDomainObjectBase, TIdent> :

        IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>,

        IAuthorizationBLLContextContainer<TAuthorizationBLLContext>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TAuthorizationBLLContext : IAuthorizationBLLContext<TIdent>
{
}

/// <summary>
/// Констекст с безопасностью
/// </summary>
/// <typeparam name="TAuthorizationBLLContext"></typeparam>
public interface ISecurityBLLContext<out TAuthorizationBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TIdent> :

        IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>,
        ISecurityBLLContext<TAuthorizationBLLContext, TPersistentDomainObjectBase, TIdent>

        where TDomainObjectBase : class
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TAuthorizationBLLContext : IAuthorizationBLLContext<TIdent>
{
}
