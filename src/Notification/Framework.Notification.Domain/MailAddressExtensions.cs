using System.Net.Mail;

namespace Framework.Notification.Domain;

internal static class MailAddressExtensions
{
    public static MailAddress ToMailAddress(this string address)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));

        return new MailAddress(address);
    }

    public static ReceiverAddressInfo ToReceiverAddress(this string address, ReceiverRole role = ReceiverRole.To)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));

        return new ReceiverAddressInfo(address.ToMailAddress(), role);
    }

    public static IEnumerable<ReceiverAddressInfo> ToReceiverAddresses(this IEnumerable<string> addresses, ReceiverRole role = ReceiverRole.To)
    {
        if (addresses == null) throw new ArgumentNullException(nameof(addresses));

        return addresses.Select(address => address.ToReceiverAddress(role));
    }
}
