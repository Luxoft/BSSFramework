using System;
using System.Linq;

namespace Framework.DomainDriven.DBGenerator
{
    public class UserCredential
    {
        private readonly string name;

        private readonly string password;

        internal UserCredential(string name, string password)
        {
            var parameters = new[] { name, password };
            if (parameters.Any(z => null == z) && parameters.Any(z => null != z))
            {
                throw new ArgumentException(
                    $"Incorrect parameter values: userName:'{this.name}', password:'{password}'");
            }

            this.name = name;
            this.password = password;
        }

        public bool IsDefault => null == this.password && null == this.name;

        public virtual string UserName => this.name;

        public virtual string Password => this.password;

        public static UserCredential CreateDefault() => new UserCredential(null, null);

        public static UserCredential Create(string name, string password) => new UserCredential(name, password);
    }
}
