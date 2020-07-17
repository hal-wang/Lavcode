using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.Controls
{
    public sealed partial class TitleBar : ContentControl
    {
        public TitleBar()
        {
            this.InitializeComponent();

            Loaded += TitleBar_Loaded;
        }

        private void TitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            Set();
        }

        public void Set()
        {
            Window.Current.SetTitleBar(this);
        }
    }
}
