namespace Framework.Core.Services;

public static class UserAuthenticationService
{
    public static IUserAuthenticationService CreateFor(string userName)
    {
        return new UserNameAuthenticationService(userName);
    }

    private class UserNameAuthenticationService(string userName) : IUserAuthenticationService
    {
        public string GetUserName()
        {
            return userName;
        }
    }
}
