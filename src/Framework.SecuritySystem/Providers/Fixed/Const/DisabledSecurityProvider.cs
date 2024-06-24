namespace Framework.SecuritySystem
{
    public class DisabledSecurityProvider<TDomainObject> : ConstSecurityProvider<TDomainObject>
    {
        public DisabledSecurityProvider()
            : base(true)
        {
        }
    }
}
