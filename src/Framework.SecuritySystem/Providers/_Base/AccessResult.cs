using System;

using Framework.SecuritySystem.AccessDeniedExceptionService;

namespace Framework.SecuritySystem;

public abstract record AccessResult
{
    public record AccessGrantedResult : AccessResult
    {
        public override AccessResult Or(Func<AccessResult> _) => this;

        public override AccessResult And(Func<AccessResult> getOtherAccessResult) => getOtherAccessResult();
    }

    public record AccessDeniedResult(Exception Reason) : AccessResult
    {
        public override AccessResult Or(Func<AccessResult> getOtherAccessResult) => getOtherAccessResult();

        public override AccessResult And(Func<AccessResult> _) => this;
    }

    public static AccessResult Create(bool grantAccess, Func<string> buildAccessDeniedMessage)
    {
        return Create(grantAccess, () => new AccessDeniedException(buildAccessDeniedMessage()));
    }

    public static AccessResult Create(bool grantAccess, Func<Exception> buildAccessDeniedException)
    {
        if (grantAccess)
        {
            return new AccessGrantedResult();
        }
        else
        {
            return new AccessDeniedResult(buildAccessDeniedException());
        }
    }

    public abstract AccessResult And(Func<AccessResult> getOtherAccessResult);

    public abstract AccessResult Or(Func<AccessResult> getOtherAccessResult);
}
