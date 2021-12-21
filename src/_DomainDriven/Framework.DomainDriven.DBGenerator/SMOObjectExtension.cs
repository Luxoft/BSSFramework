using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator
{
    public static class SMOObjectExtension
    {
        public static void CreateOrAlter(this Table source)
        {
            if(source.State == SqlSmoState.Creating)
            {
                source.Create();
                return;
            }
            if (source.State == SqlSmoState.Existing)
            {
                source.Alter();
                return;
            }
        }
    }
}