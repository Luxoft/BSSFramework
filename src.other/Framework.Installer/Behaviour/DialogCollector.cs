using WixSharp;
using WixSharp.Controls;

namespace Framework.Installer.Behaviour
{
    internal sealed class DialogCollector
    {
        internal DialogSequence Collect()
        {
            return new DialogSequence()
                .On(NativeDialogs.WelcomeDlg, Buttons.Next, new ShowDialog(NativeDialogs.VerifyReadyDlg))
                .On(NativeDialogs.VerifyReadyDlg, Buttons.Back, new ShowDialog(NativeDialogs.WelcomeDlg));
        }
    }
}