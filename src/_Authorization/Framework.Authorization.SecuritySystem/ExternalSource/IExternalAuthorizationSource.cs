﻿using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public interface IAuthorizationExternalSource
{
    IAuthorizationTypedExternalSourceBase GetTyped(SecurityContextType securityContextType, bool withCache = true);
}
