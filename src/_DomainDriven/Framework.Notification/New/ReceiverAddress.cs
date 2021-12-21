using System.Net.Mail;
using System.Text;

namespace Framework.Notification.New
{
    public class ReceiverAddress : MailAddress
    {
        public ReceiverAddress(string address, ReceiverRole role)
            : base(address)
        {
            this.Role = role;
        }

        public ReceiverAddress(string address, string displayName, ReceiverRole role)
            : base(address, displayName)
        {
            this.Role = role;
        }

        public ReceiverAddress(string address, string displayName, Encoding displayNameEncoding, ReceiverRole role)
            : base(address, displayName, displayNameEncoding)
        {
            this.Role = role;
        }


        public ReceiverRole Role { get; private set; }


        public override string ToString()
        {
            return $"{base.ToString()} (Role = {this.Role})";
        }
    }
}