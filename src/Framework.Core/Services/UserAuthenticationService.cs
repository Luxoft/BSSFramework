namespace Framework.Core.Services
{
    public static class UserAuthenticationService
    {
        public static IUserAuthenticationService CreateFor(string userName)
        {
            return new UserNameAuthenticationService(userName);
        }

        private class UserNameAuthenticationService : IUserAuthenticationService
        {
            private readonly string userName;

            public UserNameAuthenticationService(string userName)
            {
                this.userName = userName;
            }

            public string GetUserName()
            {
                return this.userName;
            }
        }
    }
}
