using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Auth
{
    public sealed partial class WindowsHelloDisabled : UserControl
    {
        public WindowsHelloDisabled()
        {
            this.InitializeComponent();
        }

        public event TypedEventHandler<WindowsHelloDisabled, Windows.UI.Xaml.RoutedEventArgs> OnRetry;

        private void RetryButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            OnRetry?.Invoke(this, e);
        }

        private void ReferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            ReferenceTip.IsOpen = true;
        }
    }
}
