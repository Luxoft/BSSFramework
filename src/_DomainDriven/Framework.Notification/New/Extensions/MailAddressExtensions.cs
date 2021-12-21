using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Framework.Notification.New
{
    internal static class MailAddressExtensions
    {
        public static MailAddress ToMailAddress(this string address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            return new MailAddress(address);
        }

        public static ReceiverAddress ToReceiverAddress(this string address, ReceiverRole? role = null)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            return new ReceiverAddress(address, role ?? ReceiverRole.To);
        }

        public static IEnumerable<ReceiverAddress> ToReceiverAddresses(this IEnumerable<string> addresses)
        {
            if (addresses == null) throw new ArgumentNullException(nameof(addresses));

            return addresses.Select(address => address.ToReceiverAddress());
        }
    }
}