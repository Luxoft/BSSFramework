﻿using Framework.Persistent;

namespace Framework.SecuritySystem;

public interface IPrincipal<out TIdent> : IVisualIdentityObject
{
    IEnumerable<IPermission<TIdent>> Permissions { get; }
}
