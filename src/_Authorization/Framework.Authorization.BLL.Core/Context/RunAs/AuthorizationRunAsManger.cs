using Framework.Authorization.Domain;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Core;

namespace Framework.Authorization.BLL;

public class AuthorizationRunAsManger : BLLContextContainer<IAuthorizationBLLContext>, IRunAsManager
{
    private readonly object _locker = new object();

    private Tuple<string, bool> _cache;


    public AuthorizationRunAsManger(IAuthorizationBLLContext context)
            : base (context)
    {

    }


    public void StartRunAsUser(string principalName)
    {
        if (principalName == null) throw new ArgumentNullException(nameof(principalName));

        lock (this._locker)
        {
            this.Context.CheckAccess(AuthorizationSecurityOperation.AuthorizationImpersonate, false);

            if (string.Equals(principalName, this._cache.Maybe(v => v.Item1), StringComparison.CurrentCultureIgnoreCase))
            {

            }
            else if (string.Equals(principalName, this.Context.CurrentPrincipalName, StringComparison.CurrentCultureIgnoreCase))
            {
                this.FinishRunAsUser();
            }
            else
            {
                var bll = this.Context.Logics.Default.Create<Principal>();

                this.Context.CurrentPrincipal.RunAs = bll.GetByName(principalName, true);

                bll.Save(this.Context.CurrentPrincipal);

                this._cache = Tuple.Create(principalName, true);
            }
        }
    }

    public void FinishRunAsUser()
    {
        lock (this._locker)
        {
            var bll = this.Context.Logics.Default.Create<Principal>();

            this.Context.CurrentPrincipal.RunAs = null;

            bll.Save(this.Context.CurrentPrincipal);

            this._cache = new Tuple<string, bool>(this.Context.CurrentPrincipalName, false);
        }
    }


    public string PrincipalName
    {
        get
        {
            lock (this._locker)
            {
                if (this._cache == null)
                {
                    this.UpdateCache();
                }

                return this._cache.Item1;
            }
        }
    }

    public bool IsRunningAs
    {
        get
        {
            lock (this._locker)
            {
                if (this._cache == null)
                {
                    this.UpdateCache();
                }

                return this._cache.Item2;
            }
        }
    }



    private void UpdateCache()
    {
        var currentPrincipal = this.Context.CurrentPrincipal;

        var impersonate = currentPrincipal.RunAs != null && currentPrincipal.RunAs != currentPrincipal;

        this._cache = Tuple.Create(impersonate ? currentPrincipal.RunAs.Name : currentPrincipal.Name, impersonate);
    }
}
